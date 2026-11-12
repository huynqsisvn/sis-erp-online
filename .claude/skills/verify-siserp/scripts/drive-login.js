// Lai xe man hinh dang nhap SIS ERP Online (Blazor Server) bang Playwright cho Node.
// Repo KHONG co test/script san cho tinh nang nay; file nay chi dung selector lay tu
// SisCom/SisCom/Pages/LoginComponent.razor (xem features/dang-nhap.md).
//
// Dung (mat khau di qua bien moi truong, KHONG dua len argv):
//   NODE_PATH="$(npm root -g)" VERIFY_USER=... VERIFY_PASS=... \
//     node drive-login.js --base https://localhost:1170 --expect success|failure --out <thu muc>
//
// Ma thoat: 0 dat ky vong | 1 sai ky vong | 2 khong toi duoc ung dung (BLOCKED) | 3 can TOTP
const fs = require('fs');
const path = require('path');
const { chromium } = require('playwright');

const args = Object.fromEntries(
    process.argv.slice(2).reduce((acc, cur, i, all) => {
        if (cur.startsWith('--')) acc.push([cur.slice(2), all[i + 1]]);
        return acc;
    }, [])
);
const base = (args.base || 'https://localhost:1170').replace(/\/$/, '');
const expectKind = args.expect || 'success';
const outDir = args.out || '.';
const user = process.env.VERIFY_USER || '';
const pass = process.env.VERIFY_PASS || '';
// Khoa 'errnameorpass' trong SISERPSME/wwwroot/Lang/vi.xml
const ERR_VI = 'Tên đăng nhập hoặc mật khẩu không đúng';

const S = {
    form: 'form.login-formv',
    user: 'form.login-formv .inputWithIcon input[type="text"]',
    pass: '#tbPassword',
    submit: '#btnlogin',
    message: 'h6.text-danger',
    totpInput: 'input[maxlength="6"]',
};

(async () => {
    fs.mkdirSync(outDir, { recursive: true });
    const result = { base, expect: expectKind, user, steps: [] };
    const finish = (code, status, detail) => {
        result.status = status; result.detail = detail; result.exitCode = code;
        fs.writeFileSync(path.join(outDir, `drive-${expectKind}.json`), JSON.stringify(result, null, 2));
        console.log(`${status}: ${detail}`);
        process.exit(code);
    };

    let browser;
    try {
        browser = await chromium.launch();
        const context = await browser.newContext({ ignoreHTTPSErrors: true, viewport: { width: 1280, height: 800 } });
        const page = await context.newPage();

        try {
            await page.goto(base + '/', { waitUntil: 'load', timeout: 30000 });
        } catch (e) {
            return finish(2, 'BLOCKED', `khong mo duoc ${base}/: ${e.message.split('\n')[0]}`);
        }
        await page.waitForSelector(S.form, { timeout: 30000 });
        // Blazor Server chi nhan su kien sau khi circuit ket noi xong.
        await page.waitForFunction(() => window.Blazor !== undefined, null, { timeout: 30000 });
        await page.screenshot({ path: path.join(outDir, `01-login-form-${expectKind}.png`) });

        await page.fill(S.user, user);
        await page.fill(S.pass, pass);
        await page.click(S.submit);

        const outcome = await Promise.race([
            page.waitForFunction(() => location.pathname !== '/', null, { timeout: 20000 }).then(() => 'navigated'),
            page.waitForSelector(S.totpInput, { timeout: 20000 }).then(() => 'totp'),
            page.waitForFunction(sel => {
                const el = document.querySelector(sel);
                return el && el.textContent.trim().length > 0;
            }, S.message, { timeout: 20000 }).then(() => 'message'),
        ]).catch(() => 'timeout');

        await page.screenshot({ path: path.join(outDir, `02-after-submit-${expectKind}.png`) });
        const message = (await page.locator(S.message).first().textContent().catch(() => '') || '').trim();
        const sessionGuid = await page.evaluate(() => localStorage.getItem('session_guid')).catch(() => null);
        Object.assign(result, { outcome, message, finalUrl: page.url(), sessionGuid });

        if (outcome === 'totp') return finish(3, 'TOTP_REQUIRED', 'tai khoan khong phai admin va TOTP dang bat');
        if (expectKind === 'failure') {
            const ok = outcome === 'message' && message.includes(ERR_VI) && new URL(page.url()).pathname === '/';
            return finish(ok ? 0 : 1, ok ? 'PASS' : 'FAIL', `outcome=${outcome} message="${message}" url=${page.url()}`);
        }
        const ok = outcome === 'navigated' && new URL(page.url()).pathname !== '/';
        return finish(ok ? 0 : 1, ok ? 'PASS' : 'FAIL', `outcome=${outcome} message="${message}" url=${page.url()}`);
    } catch (e) {
        return finish(1, 'FAIL', `ngoai le: ${e.message.split('\n')[0]}`);
    } finally {
        if (browser) await browser.close();
    }
})();
