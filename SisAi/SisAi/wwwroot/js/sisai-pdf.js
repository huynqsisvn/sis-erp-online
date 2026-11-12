// SisAi PDF Drop & Parse Helper
// Uses PDF.js (loaded via CDN in host page) to extract text from dropped PDF files
// Enhanced: position-aware text extraction for Vietnamese ERP voucher PDFs

window.SisAiPdf = {
    // Ensure PDF.js worker is set
    _initPdfJs: function () {
        if (window.pdfjsLib && !window.pdfjsLib.GlobalWorkerOptions.workerSrc) {
            window.pdfjsLib.GlobalWorkerOptions.workerSrc =
                'lib/vendor/pdf.worker.min.mjs';
        }
    },

    // Register drag-and-drop events on the chat container element
    // dotNetRef: DotNetObjectReference to call back into Blazor
    registerDropZone: function (elementId, dotNetRef) {
        var el = document.getElementById(elementId);
        if (!el) return;

        el.addEventListener('dragover', function (e) {
            e.preventDefault();
            e.stopPropagation();
            el.classList.add('pdf-drop-active');
        });

        el.addEventListener('dragleave', function (e) {
            e.preventDefault();
            e.stopPropagation();
            el.classList.remove('pdf-drop-active');
        });

        el.addEventListener('drop', function (e) {
            e.preventDefault();
            e.stopPropagation();
            el.classList.remove('pdf-drop-active');

            var files = e.dataTransfer.files;
            if (!files || files.length === 0) return;

            var file = files[0];
            if (!file.name.toLowerCase().endsWith('.pdf')) {
                dotNetRef.invokeMethodAsync('OnPdfDropError', 'Ch? h? tr? file PDF. Vui l�ng th? file .pdf.');
                return;
            }

            dotNetRef.invokeMethodAsync('OnPdfDropStart', file.name);
            SisAiPdf._readPdf(file, dotNetRef);
        });
    },

    // Build structured text from PDF text items using transform coordinates.
    // Groups items into lines by Y-coordinate, sorts within each line by X,
    // and inserts TAB separators between items that have large X gaps (table columns).
    _buildStructuredText: function (textContent) {
        var items = textContent.items;
        if (!items || items.length === 0) return '';

        // Collect items with position info
        var positioned = [];
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (!item.str || item.str.trim().length === 0) continue;
            // transform: [scaleX, skewX, skewY, scaleY, translateX, translateY]
            var tx = item.transform ? item.transform[4] : 0;
            var ty = item.transform ? item.transform[5] : 0;
            var fontSize = item.transform ? Math.abs(item.transform[3]) : 10;
            var w  = item.width || 0;
            positioned.push({ str: item.str, x: tx, y: ty, w: w, fontSize: fontSize });
        }

        if (positioned.length === 0) return '';

        // Compute adaptive Y threshold based on median font size
        var fontSizes = positioned.map(function (p) { return p.fontSize; }).sort(function (a, b) { return a - b; });
        var medianFontSize = fontSizes[Math.floor(fontSizes.length / 2)] || 10;
        var yThreshold = Math.max(medianFontSize * 0.6, 3);

        // Sort by Y descending (PDF coords: bottom=0), then X ascending
        positioned.sort(function (a, b) {
            var dy = b.y - a.y;
            if (Math.abs(dy) > yThreshold) return dy > 0 ? 1 : -1;
            return a.x - b.x;
        });

        // Group into lines: items within yThreshold Y are on the same line
        var lines = [];
        var currentLine = [positioned[0]];
        var currentY = positioned[0].y;

        for (var i = 1; i < positioned.length; i++) {
            if (Math.abs(positioned[i].y - currentY) <= yThreshold) {
                currentLine.push(positioned[i]);
            } else {
                currentLine.sort(function (a, b) { return a.x - b.x; });
                lines.push(currentLine);
                currentLine = [positioned[i]];
                currentY = positioned[i].y;
            }
        }
        if (currentLine.length > 0) {
            currentLine.sort(function (a, b) { return a.x - b.x; });
            lines.push(currentLine);
        }

        // Compute adaptive TAB gap based on median character width
        var charWidths = positioned.filter(function (p) { return p.str.length > 0 && p.w > 0; })
            .map(function (p) { return p.w / p.str.length; });
        charWidths.sort(function (a, b) { return a - b; });
        var medianCharW = charWidths.length > 0 ? charWidths[Math.floor(charWidths.length / 2)] : 5;
        var tabGapThreshold = Math.max(medianCharW * 4, 15);

        // Build text: join items in each line; use TAB for large gaps (table columns)
        var result = [];
        for (var li = 0; li < lines.length; li++) {
            var line = lines[li];
            var lineStr = '';
            for (var j = 0; j < line.length; j++) {
                if (j > 0) {
                    var gap = line[j].x - (line[j - 1].x + line[j - 1].w);
                    if (gap > tabGapThreshold) {
                        lineStr += '\t';
                    } else if (gap > 1) {
                        lineStr += ' ';
                    }
                    // gap <= 1: items are adjacent (e.g. "2" + "." + "000"), concatenate directly
                }
                lineStr += line[j].str;
            }
            result.push(lineStr.trim());
        }

        return result.join('\n');
    },

    // Read PDF file and extract all text with position-aware layout
    _readPdf: function (file, dotNetRef) {
        SisAiPdf._initPdfJs();

        if (!window.pdfjsLib) {
            dotNetRef.invokeMethodAsync('OnPdfDropError',
                'Th? vi?n PDF.js ch?a ???c t?i. Vui l�ng t?i l?i trang v� th? l?i.');
            return;
        }

        var self = this;
        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            var typedArray = new Uint8Array(e.target.result);

            window.pdfjsLib.getDocument({ data: typedArray }).promise.then(function (pdf) {
                var totalPages = pdf.numPages;
                var pagePromises = [];

                for (var i = 1; i <= totalPages; i++) {
                    pagePromises.push(pdf.getPage(i).then(function (page) {
                        return page.getTextContent().then(function (textContent) {
                            // Use position-aware extraction
                            return self._buildStructuredText(textContent);
                        });
                    }));
                }

                Promise.all(pagePromises).then(function (pages) {
                    var fullText = pages.join('\n');
                    dotNetRef.invokeMethodAsync('OnPdfTextExtracted', file.name, fullText);
                }).catch(function (err) {
                    dotNetRef.invokeMethodAsync('OnPdfDropError', 'L?i ??c trang PDF: ' + err.message);
                });

            }).catch(function (err) {
                dotNetRef.invokeMethodAsync('OnPdfDropError', 'Kh�ng th? m? file PDF: ' + err.message);
            });
        };

        fileReader.onerror = function () {
            dotNetRef.invokeMethodAsync('OnPdfDropError', 'L?i ??c file. Vui l�ng th? l?i.');
        };

        fileReader.readAsArrayBuffer(file);
    },

    // Trigger file input click (for button-based selection)
    triggerFilePicker: function (inputId) {
        var input = document.getElementById(inputId);
        if (input) input.click();
    },

    // Handle file input change event
    handleFileInputChange: function (inputId, dotNetRef) {
        var input = document.getElementById(inputId);
        if (!input) return;

        input.addEventListener('change', function () {
            if (!input.files || input.files.length === 0) return;
            var file = input.files[0];
            if (!file.name.toLowerCase().endsWith('.pdf')) {
                dotNetRef.invokeMethodAsync('OnPdfDropError', 'Ch? h? tr? file PDF.');
                return;
            }
            dotNetRef.invokeMethodAsync('OnPdfDropStart', file.name);
            SisAiPdf._readPdf(file, dotNetRef);
            // Reset input so same file can be re-selected
            input.value = '';
        });
    }
};
