window.SisAiVoice = (function () {
    let recognition = null;
    let dotNetRef = null;

    function isSupported() {
        return !!(window.SpeechRecognition || window.webkitSpeechRecognition);
    }

    function start(ref) {
        if (!isSupported()) return;

        // Clean up previous instance
        if (recognition) {
            recognition.onend = null;
            recognition.onerror = null;
            recognition.onresult = null;
            try { recognition.abort(); } catch (e) { }
            recognition = null;
        }

        dotNetRef = ref;

        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        recognition = new SpeechRecognition();
        recognition.lang = 'vi-VN';
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;
        recognition.continuous = false;

        let resultReceived = false;

        recognition.onresult = function (event) {
            resultReceived = true;
            recognition.onerror = null;
            recognition.onend = null;
            const transcript = event.results[0][0].transcript;
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnVoiceResult', transcript);
            }
        };

        recognition.onerror = function (event) {
            if (resultReceived) return;
            recognition.onend = null;
            const err = event.error;
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnVoiceError', err);
            }
        };

        recognition.onend = function () {
            if (resultReceived) return;
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnVoiceError', 'ended');
            }
        };

        recognition.start();
    }

    function stop() {
        if (recognition) {
            recognition.onend = null;
            recognition.onerror = null;
            recognition.onresult = null;
            try { recognition.abort(); } catch (e) { }
            recognition = null;
        }
        dotNetRef = null;
    }

    return {
        isSupported: isSupported,
        start: start,
        stop: stop
    };
})();

