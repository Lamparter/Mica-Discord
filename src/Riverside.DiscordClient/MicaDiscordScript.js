//@ts-check
(function () {
    if (window.location.hostname != "discord.com") return;

    /**
     * @typedef EventData
     * @property {string} data
    */

    /**
     * @typedef WebView2
     * @property {(arg0 : string) => void} postMessage
     * @property {(arg0 : string, arg1 : (arg0 : EventData) => void) => void} addEventListener
    */

    /**
     * @typedef CustomWindow
     * @property {{webview? : WebView2}} chrome
    */
    const Window = (/** @type {CustomWindow} */ ((/** @type {Object} */ (window))));

    const WebView = Window.chrome.webview;
    delete Window.chrome.webview; // web apps should have no access to window.chrome.webview

    const html = document.getElementsByTagName('html')[0];
    let dark = html.classList.contains('theme-dark');
    new MutationObserver(function (mutations, observer) {
        if (html.classList.contains('theme-dark')) {
            if (dark) return;
            dark = true;
            WebView.postMessage('dark');
        }
        if (html.classList.contains('theme-light')) {
            if (!dark) return;
            dark = false;
            WebView.postMessage('light');
        }
    }).observe(html, { attributes: true });
})()