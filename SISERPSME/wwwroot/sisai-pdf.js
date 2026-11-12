// SisAi PDF Drop & Parse Helper
// Uses PDF.js (loaded via CDN in host page) to extract text from dropped PDF files

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

    // Read PDF file and extract all text
    _readPdf: function (file, dotNetRef) {
        SisAiPdf._initPdfJs();

        if (!window.pdfjsLib) {
            dotNetRef.invokeMethodAsync('OnPdfDropError',
                'Th? vi?n PDF.js ch?a ???c t?i. Vui l�ng t?i l?i trang v� th? l?i.');
            return;
        }

        var fileReader = new FileReader();
        fileReader.onload = function (e) {
            var typedArray = new Uint8Array(e.target.result);

            window.pdfjsLib.getDocument({ data: typedArray }).promise.then(function (pdf) {
                var totalPages = pdf.numPages;
                var pagePromises = [];

                for (var i = 1; i <= totalPages; i++) {
                    pagePromises.push(pdf.getPage(i).then(function (page) {
                        return page.getTextContent().then(function (textContent) {
                            return textContent.items.map(function (item) {
                                return item.str;
                            }).join(' ');
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
