// ===== blazorKeyListener (removable) =====
window.blazorKeyListener = (function () {
    let keyHandler = null;
    return {
        init: function (dotNetHelper) {
            if (keyHandler) return;
            keyHandler = function (event) {
                try {
                    if (event.key === "F5" || event.key === "F3" || event.key === "F4" || event.key === "F7") {
                        event.preventDefault();
                    }
                    if (dotNetHelper && dotNetHelper.invokeMethodAsync) {
                        dotNetHelper.invokeMethodAsync("OnKeyDown", event.keyCode);
                    }
                } catch (e) { console.error(e); }
            };
            document.addEventListener("keydown", keyHandler);
        },
        remove: function () {
            if (keyHandler) {
                document.removeEventListener("keydown", keyHandler);
                keyHandler = null;
            }
        }
    };
})();

window.goBack = () => history.back();
window.focusInside = (wrapperId) => {
    const wrapper = document.getElementById(wrapperId);
    const active = document.activeElement;
    if (!wrapper || !active) return false;

    // 1. Focus còn trong grid-wrapper
    if (wrapper.contains(active)) return true;

    // 2. Kiểm tra các popup/dropdown của DevExpress
    const devexpressPopups = [
        '.dxbl-popup-content',
        '.dxbl-dropdown-popup',
        '.dxbl-combo-box-popup',
        '.dxbl-list-popup',
        '.dx-overlay-content',
        '.dxbl-popup-portal'
    ];

    for (const selector of devexpressPopups) {
        const popup = active.closest(selector);
        if (popup) {
            // Kiểm tra xem popup này có liên quan đến grid không
            const triggerElement = document.querySelector('[aria-controls="' + popup.id + '"]');
            if (triggerElement && wrapper.contains(triggerElement)) {
                return true;
            }
        }
    }

    // 3. Kiểm tra aria-controls
    const ariaId = active.getAttribute("aria-controls");
    if (ariaId) {
        const popupEl = document.getElementById(ariaId);
        if (popupEl) return true;
    }

    // 4. Kiểm tra input đang expanded trong grid
    const expandedInput = wrapper.querySelector("input[aria-expanded='true']");
    if (expandedInput) return true;

    return false;
};

window.closeDxDropDown = function (id) {
    const el = document.getElementById(id);
    if (el) el.click(); // toggle off
}

// ===== small DOM helpers =====
window.AttachGridEnterSave = function (dotnetRef) {
    document.addEventListener("keydown", async function (e) {
        if (e.key === "Enter" && e.target.closest(".dxbl-grid")) {
            e.preventDefault();
            await dotnetRef.invokeMethodAsync("SaveAndMoveNextRow");
        }
    });
};
window.changeTextGridPageSize = function (pText) {
    var myPager = document.getElementsByClassName("my-pager")[0];
    if (myPager) {
        var label = myPager.getElementsByClassName("dxbl-text")[0];
        if (label) label.innerText = pText.toString();
    }
};

window.changeTextNewRowButton = function (pText) {
    const nodes = document.querySelectorAll('.dxbl-grid-edit-new-item-row-content');
    nodes.forEach(node => {
        if (node.textContent.includes("Click here to add a new row")) {
            node.childNodes.forEach(child => {
                if (child.nodeType === Node.TEXT_NODE && child.textContent.includes("Click here to add a new row")) {
                    node.removeChild(child);
                    const span = document.createElement("span");
                    span.innerText = pText.toString();
                    span.classList.add("blink-text");
                    node.appendChild(span);
                }
            });
        }
    });
};

window.addBeforeUnloadListener = function (dotNetHelper) {
    if (!dotNetHelper) return;
    window.addEventListener("beforeunload", function () {
        try { dotNetHelper.invokeMethodAsync('OnPageClosing'); } catch (e) { }
    });
};

// Note: inline style injection may be blocked by CSP
window.SetDXGridHeaderFooter = function (hColor, hbgColor, fColor, fbgColor) {
    const style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = `.dxbl-grid-header-row { color: ${hColor}; background-color: ${hbgColor}; } .dxbl-grid-footer-row { color: ${fColor}; background-color: ${fbgColor}; }`;
    document.head.appendChild(style);
};
window.ReSetDXGridHeaderFooter = function () {
    const style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = '.dxbl-grid-header-row { color: black; background-color: transparent; } .dxbl-grid-footer-row { color: black; background-color: transparent; }';
    document.head.appendChild(style);
};

window.Movetoend = function (divName) {
    const el = document.getElementById(divName);
    if (el) el.scrollTop = el.scrollHeight;
};
window.MoveIntoView = function (divName) {
    const el = document.getElementById(divName);
    if (el) el.scrollIntoView();
};

window.SetDocumentTitle = function (title) { document.title = title || document.title; };
window.SendKeyTab = function () {
    const press = new KeyboardEvent('keydown', { key: 'Tab', keyCode: 9, which: 9 });
    document.body.dispatchEvent(press);
};
window.Focusinput = function (id) { const el = document.getElementById(id); if (el) el.focus(); };

// ===== PageObj for Blazor interop =====
var PageObj = null;
window.SetPageObj = function (obj) { PageObj = obj; };

// ===== QR / scanner / notifications / map helpers (kept lightweight) =====
window.GetQrcodeValue = function (resultdiv) {
    var el = document.getElementById(resultdiv);
    if (!el) return null;
    var result = el.innerText;
    el.innerText = '';
    return result;
};

window.ScanQrcode = function (parentdiv, resultdiv, objRef) {
    try {
        const node = document.createElement("div");
        node.setAttribute("id", "qrcodereader");
        document.getElementById(parentdiv).appendChild(node);
        const scanner = new Html5QrcodeScanner('qrcodereader', { qrbox: { width: 250, height: 250 }, fps: 20 });
        scanner.render(function success(result) {
            document.getElementById(resultdiv).innerText = result;
            scanner.clear();
            document.getElementById('qrcodereader').remove();
            if (objRef && objRef.invokeMethodAsync) objRef.invokeMethodAsync('JStoCSCall', result);
        }, function error(err) { console.error(err); });
    } catch (ex) { console.error(ex); }
};

window.ShowToast = (divName) => {
    var toastElList = Array.prototype.slice.call(document.querySelectorAll('#' + divName));
    var toastList = toastElList.map(function (toastEl) { return new bootstrap.Toast(toastEl, { delay: 10000 }); });
    toastList.forEach(toast => toast.show());
};

window.RegisterNotification = function () {
    if (!("Notification" in window)) return;
    if (Notification.permission !== 'granted') Notification.requestPermission();
};
window.BroweserPushNotification = function (title, content, url) {
    if (!("Notification" in window)) return;
    if (Notification.permission !== 'granted') Notification.requestPermission();
    else {
        const options = { body: content, dir: 'ltr', image: '../images/logo.svg' };
        const _title = title || 'S.I.S VN Thông báo';
        const notification = new Notification(_title, options);
        if (url) notification.onclick = function () { window.open(url); };
    }
};

// ===== Enter-as-Tab: document-level capture listener =====
(function () {
    function isFocusable(el) {
        if (!el) return false;
        if (el.disabled) return false;
        if (el.tabIndex === -1) return false;
        const tag = (el.tagName || '').toLowerCase();
        if (tag === 'input' || tag === 'select' || tag === 'textarea' || tag === 'button' || el.hasAttribute && el.hasAttribute('contenteditable')) {
            try {
                const cs = window.getComputedStyle(el);
                if (cs && (cs.display === 'none' || cs.visibility === 'hidden' || cs.opacity === '0')) return false;
            } catch { /* ignore */ }
            return true;
        }
        if (el.href) return true;
        return false;
    }

    function getAllFocusable(root = document) {
        const selector = 'input:not([type=hidden]):not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), a[href], [tabindex]:not([tabindex="-1"])';
        return Array.from(root.querySelectorAll(selector)).filter(isFocusable);
    }

    function findContainingTabPanel(el) {
        if (!el) return null;
        let panel = el.closest('[role="tabpanel"]');
        if (panel) return panel;
        panel = el.closest('.tab-pane, .dxbl-tab-page, .dx-tabpage, .dxbl-formlayout-tabpage');
        return panel;
    }

    function activateTabForPanel(panel) {
        if (!panel) return false;
        const id = panel.id;
        if (id) {
            let tab = document.querySelector('[role="tab"][aria-controls="' + id + '"]')
                || document.querySelector('[data-bs-target="#' + id + '"]')
                || document.querySelector('a[href="#' + id + '"]');
            if (tab) { try { tab.click(); return true; } catch { } }
        }
        try {
            const tablist = panel.parentElement && panel.parentElement.querySelector('[role="tablist"]');
            if (tablist) {
                const panels = Array.from(panel.parentElement.querySelectorAll('[role="tabpanel"]'));
                const headers = Array.from(tablist.querySelectorAll('[role="tab"]'));
                const idx = panels.indexOf(panel);
                if (idx >= 0 && headers[idx]) { try { headers[idx].click(); return true; } catch { } }
            }
        } catch { }
        return false;
    }
    // Replace only the AttachEnterAsTab implementation in your Script.js with this function.
    // This version handles DxGrid specially and also handles buttons:
    // - When Enter is pressed on a button (DxButton or regular button), it clicks the button instead of moving focus
    // - When focus is inside a grid row, Enter moves to the next cell in the same row
    // - When at row end it moves to the first editable cell of the next row (or triggers adding row)
    window.AttachEnterAsTab = function () {
        if (window.__enterAsTabAttached) return;
        window.__enterAsTabAttached = true;

        function isVisible(el) {
            if (!el) return false;
            try {
                const cs = window.getComputedStyle(el);
                if (!cs) return false;
                if (cs.display === 'none' || cs.visibility === 'hidden' || +cs.opacity === 0) return false;
            } catch { }
            return el.offsetParent !== null || el.getClientRects().length > 0;
        }

        function isFocusable(el) {
            if (!el) return false;
            if (el.disabled) return false;
            if (el.tabIndex === -1) return false;
            const t = (el.tagName || '').toLowerCase();
            if (t === 'input' || t === 'select' || t === 'textarea' || t === 'button') return true;
            if (el.href) return true;
            return el.hasAttribute && el.hasAttribute('contenteditable');
        }

        function getRowEditors(row) {
            if (!row) return [];
            const sel = 'input:not([type=hidden]):not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])';
            return Array.from(row.querySelectorAll(sel)).filter(el => isFocusable(el) && isVisible(el));
        }

        function anyPopupOpen() {
            const popupSelectors = [
                '.dx-popup-content',
                '.dx-overlay',
                '.dx-selectbox-popup-wrapper',
                '.dx-dropdowneditor-popup',
                '.dx-list',
                '.dropdown-menu'
            ];
            for (const s of popupSelectors) {
                const els = document.querySelectorAll(s);
                for (const el of els) {
                    if (isVisible(el)) return true;
                }
            }
            return false;
        }

        function isTargetDropdownExpanded(target) {
            if (!target) return false;
            try {
                if (target.getAttribute && target.getAttribute('aria-expanded') === 'true') return true;
                if (target.closest && target.closest('.dx-dropdowneditor, .dx-selectbox, .dx-autocomplete')) {
                    if (anyPopupOpen()) return true;
                }
            } catch { }
            return false;
        }

        function isButton(el) {
            if (!el) return false;
            const tag = (el.tagName || '').toLowerCase();
            if (tag === 'button') return true;
            // Check for DxButton wrapper classes
            if (el.classList && (
                el.classList.contains('dxbl-btn') ||
                el.classList.contains('dxbs-btn') ||
                el.classList.contains('dx-button') ||
                el.classList.contains('btn')
            )) return true;
            // Check if element is inside a button wrapper
            if (el.closest && (
                el.closest('button') ||
                el.closest('.dxbl-btn') ||
                el.closest('.dxbs-btn') ||
                el.closest('.dx-button')
            )) return true;
            return false;
        }

        const handler = function (e) {
            try {
                if (e.key !== 'Enter') return;
                if (e.ctrlKey || e.shiftKey || e.altKey || e.metaKey) return;

                const target = e.target;
                if (!target) return;
                const tag = (target.tagName || '').toLowerCase();
                if (tag === 'textarea') return;
                if (target.isContentEditable) return;

                // If dropdown/list popup is open (or target reports expanded) let the control handle Enter
                if (isTargetDropdownExpanded(target) || anyPopupOpen()) return;

                // CHECK IF TARGET IS A BUTTON - Click it and stop (don't move focus away)
                if (isButton(target)) {
                    e.preventDefault();
                    e.stopPropagation();
                    try {
                        // Find the actual button element to click
                        let buttonToClick = target;
                        if (tag !== 'button') {
                            buttonToClick = target.closest('button') ||
                                target.closest('.dxbl-btn') ||
                                target.closest('.dxbs-btn') ||
                                target.closest('.dx-button') ||
                                target;
                        }
                        buttonToClick.click();
                    } catch (ex) {
                        console.error('Error clicking button:', ex);
                    }
                    return; // Stay on button, don't move focus
                }

                // SPECIAL: inside a DevExpress Grid -> handle row-wise navigation
                const grid = target.closest && (target.closest('.dxbl-grid') || target.closest('.dx-grid') || target.closest('.dxbl-grid-root'));
                if (grid) {
                    // try to find the DOM row container
                    let row = target.closest('tr') || target.closest('.dxbl-grid-row') || target.closest('.dxbl-grid-data-row') || target.closest('[role="row"]');
                    if (row) {
                        const editors = getRowEditors(row);
                        // find index of currently focused editor within editors (by direct equality or containment)
                        let idx = editors.indexOf(document.activeElement);
                        if (idx === -1) {
                            for (let i = 0; i < editors.length; i++) {
                                if (editors[i] === target || editors[i].contains(target) || target.contains(editors[i])) { idx = i; break; }
                            }
                        }

                        // If we found index and there is a next editor in same row -> focus it
                        if (idx >= 0 && idx < editors.length - 1) {
                            e.preventDefault(); e.stopPropagation();
                            try { editors[idx + 1].focus(); if (typeof editors[idx + 1].select === 'function') editors[idx + 1].select(); } catch { }
                            return;
                        }

                        // If at last editor in row -> move to first editor of next data row (if any)
                        if (idx >= 0) {
                            // find next sibling row that looks like data row
                            let nextRow = row.nextElementSibling;
                            while (nextRow) {
                                // skip non-row nodes until we find a row with editors
                                const nextEditors = getRowEditors(nextRow);
                                if (nextEditors.length > 0) {
                                    e.preventDefault(); e.stopPropagation();
                                    try { nextEditors[0].focus(); if (typeof nextEditors[0].select === 'function') nextEditors[0].select(); } catch { }
                                    return;
                                }
                                nextRow = nextRow.nextElementSibling;
                            }

                            // no next row found: ask Blazor to add new row (existing CallGridEditMode handler)
                            if (window.PageObj && PageObj.invokeMethodAsync) {
                                e.preventDefault(); e.stopPropagation();
                                // keep the name used by your C# handler
                                PageObj.invokeMethodAsync('CallGridEditMode', 'end_input_GridCt0');
                                return;
                            }
                        }
                    }
                    // if grid but no row or not handled, fall through to global next-focus behavior
                }

                // GLOBAL: fallback — move to next focusable in DOM order
                e.preventDefault(); e.stopPropagation();
                const all = Array.from(document.querySelectorAll('input:not([type=hidden]):not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), a[href], [tabindex]:not([tabindex="-1"])')).filter(el => isFocusable(el) && isVisible(el));
                if (!all.length) return;
                let currentIndex = all.indexOf(document.activeElement);
                if (currentIndex === -1) {
                    for (let i = 0; i < all.length; i++) {
                        if (all[i] === target || all[i].contains(target) || target.contains(all[i])) { currentIndex = i; break; }
                    }
                }
                const nextIndex = (currentIndex >= 0) ? ((currentIndex + 1) % all.length) : 0;
                const next = all[nextIndex];
                if (!next) return;

                // If next is inside hidden tab panel attempt to activate tab
                const panel = next.closest && next.closest('[role="tabpanel"], .tab-pane, .dxbl-formlayout-tabpage');
                if (panel) {
                    let hidden = false;
                    try {
                        const cs = window.getComputedStyle(panel);
                        if (cs && (cs.display === 'none' || cs.visibility === 'hidden')) hidden = true;
                        if (panel.getAttribute && panel.getAttribute('aria-hidden') === 'true') hidden = true;
                    } catch { }
                    if (hidden) {
                        const id = panel.id;
                        let tab = null;
                        if (id) tab = document.querySelector('[role="tab"][aria-controls="' + id + '"], [data-bs-target="#' + id + '"], a[href="#' + id + '"]');
                        if (!tab) {
                            const tablist = panel.parentElement && panel.parentElement.querySelector('[role="tablist"]');
                            if (tablist) {
                                const panels = Array.from(panel.parentElement.querySelectorAll('[role="tabpanel"]'));
                                const headers = Array.from(tablist.querySelectorAll('[role="tab"]'));
                                const idxPanel = panels.indexOf(panel);
                                if (idxPanel >= 0) tab = headers[idxPanel];
                            }
                        }
                        if (tab) {
                            try { tab.click(); } catch { }
                            setTimeout(() => { try { next.focus(); if (typeof next.select === 'function') next.select(); } catch { } }, 150);
                            return;
                        }
                    }
                }

                try { next.focus(); if (typeof next.select === 'function') next.select(); } catch { }
            } catch (ex) {
                console.error('AttachEnterAsTab handler error', ex);
            }
        };

        document.addEventListener('keydown', handler, true);
        window.__enterAsTabHandler = handler;
    };

    window.ModifyEnterKeyPressAsTab = function (event) {
        if (!window.__enterAsTabAttached) window.AttachEnterAsTab();
        // capture handler will process the Enter key
    };

    window.DetachEnterAsTab = function () {
        if (!window.__enterAsTabAttached) return;
        if (window.__enterAsTabHandler) {
            document.removeEventListener('keydown', window.__enterAsTabHandler, true);
            window.__enterAsTabHandler = null;
        }
        window.__enterAsTabAttached = false;
    };
})();