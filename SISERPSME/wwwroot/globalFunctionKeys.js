// Global storage for DotNetObjectReference set by Blazor:
// invoked from Blazor: JavacriptS.InvokeVoidAsync("SetPageFModeObj", Pageobj, pageId);
window._blazorPageObj = null;
window._blazorPageObjId = null;

window.SetPageFModeObj = function (dotNetRef, id) {
    window._blazorPageObj = dotNetRef;
    window._blazorPageObjId = id || null;
};

// Key handler: captures F1..F12 and Escape, forwards to Blazor instance method "GridFMode"
window._handleFunctionKeys = function (e) {
    try {
        const key = (e.key || e.code || "").toString();
        if (!key) return;

        const k = key.toUpperCase();

        // Accept Escape or F1..F12
        const isFunctionKey =
            k === "ESCAPE" ||
            (k.startsWith("F") && (() => {
                const n = parseInt(k.substring(1), 10);
                return !isNaN(n) && n >= 1 && n <= 12;
            })());

        if (!isFunctionKey) return;

        // Do not intercept when user is typing in inputs, textareas, selects or contenteditable
        const target = e.target || {};
        const tag = (target.tagName || "").toString().toLowerCase();
        const editable = target.isContentEditable || tag === "input" || tag === "textarea" || tag === "select";
        if (editable) return;

        // Forward to Blazor if available
        if (window._blazorPageObj && typeof window._blazorPageObj.invokeMethodAsync === "function") {
            const value = k === "ESCAPE" ? "ESCAPE" : k; // e.g., "F5"
            // prevent default browser behavior for F-keys (e.g., F1 help)
            e.preventDefault();
            e.stopPropagation();
            window._blazorPageObj.invokeMethodAsync("GridFMode", value, window._blazorPageObjId).catch(err => {
                console && console.error && console.error("GridFMode invoke failed", err);
            });
        }
    } catch (err) {
        console && console.error && console.error("globalFunctionKeys error", err);
    }
};

// Register handler (capture phase to intercept early)
window.addEventListener("keydown", window._handleFunctionKeys, true);

// Utility to remove handler if needed
window.RemoveGlobalFunctionKeyHandler = function () {
    window.removeEventListener("keydown", window._handleFunctionKeys, true);
    window._blazorPageObj = null;
    window._blazorPageObjId = null;
};