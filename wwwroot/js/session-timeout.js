(function () {
    const body = document.body;
    const isAuthenticated = body && body.getAttribute('data-authenticated') === 'true';
    if (!isAuthenticated) {
        return;
    }

    const inactivityLimitMs = 60_000; // 1 minute for testing
    const warningOffsetMs = 15_000; // show modal 15s before
    let warningTimer = null;
    let expiryTimer = null;
    let countdownInterval = null;
    let expiryTime = null;
    let warningVisible = false;

    const modalElement = document.getElementById('sessionTimeoutModal');
    const countdownElement = document.getElementById('session-timeout-countdown');
    const extendButton = document.getElementById('extend-session-btn');

    if (!modalElement || !countdownElement || !extendButton) {
        return;
    }

    const modal = new bootstrap.Modal(modalElement, { backdrop: 'static', keyboard: false });

    function cleanupModalArtifacts() {
        document.body.classList.remove('modal-open');
        document.body.style.removeProperty('padding-right');
        document.querySelectorAll('.modal-backdrop').forEach(el => el.remove());
    }

    function clearTimers() {
        if (warningTimer) {
            clearTimeout(warningTimer);
            warningTimer = null;
        }
        if (expiryTimer) {
            clearTimeout(expiryTimer);
            expiryTimer = null;
        }
        if (countdownInterval) {
            clearInterval(countdownInterval);
            countdownInterval = null;
        }
    }

    function hideModal() {
        if (modalElement.classList.contains('show')) {
            modal.hide();
        }
        cleanupModalArtifacts();
        warningVisible = false;
    }

    function scheduleTimers() {
        clearTimers();
        hideModal();
        const now = Date.now();
        expiryTime = now + inactivityLimitMs;
        const warningIn = inactivityLimitMs - warningOffsetMs;
        warningTimer = setTimeout(showWarning, warningIn);
        expiryTimer = setTimeout(expireSession, inactivityLimitMs);
    }

    function showWarning() {
        warningVisible = true;
        const updateCountdown = () => {
            const remaining = Math.max(0, Math.ceil((expiryTime - Date.now()) / 1000));
            countdownElement.textContent = remaining.toString();
        };

        updateCountdown();
        countdownInterval = setInterval(updateCountdown, 1000);
        modal.show();
    }

    async function extendSession() {
        try {
            await fetch('/Account/KeepAlive', { method: 'POST' });
        } catch {
            // ignore errors; fallback to rescheduling anyway
        }
        warningVisible = false;
        hideModal();
        scheduleTimers();
    }

    async function expireSession() {
        warningVisible = false;
        hideModal();
        try {
            await fetch('/Account/LogoutIdle', { method: 'POST' });
        } catch {
            // ignore errors; redirect anyway
        }
        window.location.href = '/Account/Login?expired=1';
    }

    function resetActivity() {
        if (warningVisible) {
            return;
        }
        scheduleTimers();
    }

    extendButton.addEventListener('click', extendSession);

    ['click', 'mousemove', 'keydown', 'scroll', 'touchstart'].forEach(evt => {
        window.addEventListener(evt, resetActivity, { passive: true });
    });

    scheduleTimers();
})();
