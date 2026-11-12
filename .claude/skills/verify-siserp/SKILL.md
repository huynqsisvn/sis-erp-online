---
name: verify-siserp
description: Dựng, kiểm tra sẵn sàng, thao tác và dọn dẹp SIS ERP Online (Blazor Server, .NET 8, SQL Server) để xác minh một thay đổi bằng chạy thật. Dùng khi cần xác nhận ứng dụng chạy hoặc đăng nhập hoạt động; không dùng để sửa mã.
---

# Xác minh SIS ERP Online

Ứng dụng web: `SISERPSME/SISERPSME.csproj` (`Microsoft.NET.Sdk.Web`, `net8.0`, Blazor Server + DevExpress 24.2.3).
Giải pháp `SISERPSME.sln` chỉ gồm 4 project: `SISERPSME`, `SisSo`, `SisLib`, `SisCom`.
Lệnh trong file này lấy từ `global.json`, `*.csproj`, `Program.cs`, `launchSettings.json`.

## Trạng thái đã xác minh (lần chạy 2026-11-12, commit `a6e842b`)

Chi tiết: [`evidence/2026-11-12-run1.md`](evidence/2026-11-12-run1.md).

| Phần | Đã chạy thật? |
|---|---|
| `scripts/doctor.sh` | **Có.** Thoát 1, báo 3 blocker bên dưới |
| Restore/build thử trên bản sao tạm (đã bỏ `global.json`, SDK 8) | **Có**, chỉ để khảo sát: restore thoát 0, build thoát 1 (144 lỗi) |
| `dotnet restore/build` đúng theo repo | **Không chạy được:** SDK `9.0.312` không cài được trong sandbox |
| `dotnet restore`/`build`/`run` theo các khối lệnh bên dưới | **Có chạy, thất bại:** cả ba thoát 145 (thiếu SDK 9.0.312) |
| Khởi động ứng dụng, readiness | **Chưa đạt:** tiến trình thoát ngay, cổng 1170 không mở, readiness `000` |
| Cơ chế PID/PGID, kiểm tra chủ cổng, dừng nhóm | **Có**, thử bằng máy chủ giả (không phải ứng dụng thật) |
| `scripts/drive-login.js` | **Chạy một phần:** Chromium mở được, ứng dụng không chạy nên thoát 2 (`BLOCKED`) cho cả đúng lẫn sai thông tin |
| Đăng nhập đúng và sai thông tin | **Chưa xác minh** (cần ứng dụng và DB) |
| Cleanup | **Có:** không có tiến trình cần dừng, cổng 1169/1170 trống, `created-by-run.txt` rỗng; không có dữ liệu DB để dọn |

Đừng báo "đã xác minh" cho bất kỳ mục nào ghi "Chưa chạy". Báo đúng là "chưa chạy được vì …".

### Blocker đã biết (baseline, không phải hồi quy)

1. **SDK:** `global.json` ghim `9.0.312` (`rollForward: latestMinor`), nhưng mọi project nhắm `net8.0`.
   Sandbox chỉ cài được SDK 8.0 (apt); host tải SDK 9 bị chặn.
2. **DLL dựng sẵn không có trong repo:** `SISERPSME.csproj` và các project khác tham chiếu
   `Dll\Debug\net8.0\*.dll` (`SisData`, `SisFunc`, `SisSm`, `RestSharp`, `RemoteCommand`, `SasClsEinvoice` và các DLL của
   `SisCa/SisCo/SisCom/SisIn/SisLib/SisSo`). `SisData`, `SisFunc`, `SisSm` không có mã nguồn trong repo.
   Thiếu chúng thì build lỗi `CS0246` (đã quan sát: 144 lỗi). Chưa rõ nguồn lấy; hỏi chủ repo.
3. **SQL Server:** chuỗi kết nối `ConnectionStrings:Default` trong `SISERPSME/appsettings.json` trỏ tới một máy SQL Server
   nội bộ (tên máy ở `Server=`). Máy này không phân giải được từ sandbox.
4. Khác: DevExpress `24.2.3` không có trên nuget.org; restore tự chọn `25.1.3` (cảnh báo `NU1603`).
   Feed riêng của DevExpress không truy cập được từ sandbox. Repo không có `nuget.config`.

> **Bảo mật:** `appsettings.json` đang chứa tài khoản `sa` và mật khẩu ở dạng rõ, đã commit.
> Không sao chép vào log, báo cáo hay PR. Không dùng DB đó để chạy thử.

## Launch

Mọi lệnh chạy từ gốc repo. Mỗi lần chạy có `RUN_ID` riêng.

```bash
export RUN_ID="$(date -u +%Y%m%dT%H%M%SZ)-$$"
export RUN_DIR="${TMPDIR:-/tmp}/verify-siserp/$RUN_ID"
mkdir -p "$RUN_DIR/screenshots"

# 0) Doctor: dừng nếu thoát khác 0 và báo blocker, đừng tự sửa
bash .claude/skills/verify-siserp/scripts/doctor.sh "$RUN_DIR"; echo "doctor exit: $?" | tee "$RUN_DIR/doctor.exit"
```

Chỉ tiếp tục khi `doctor.exit` là 0.

```bash
# 1) Cài dependency (NuGet). Repo không có package.json/.node-version.
export DOTNET_CLI_TELEMETRY_OPTOUT=1 DOTNET_NOLOGO=1
dotnet restore SISERPSME.sln > "$RUN_DIR/restore.log" 2>&1; echo "restore exit: $?" | tee "$RUN_DIR/restore.exit"

# 2) Build. Ghi vào Dll/, bin/, obj/ ở gốc repo; .gitignore KHÔNG bỏ qua các thư mục này,
#    nên phải dọn ở bước Cleanup.
dotnet build SISERPSME.sln --no-restore > "$RUN_DIR/build.log" 2>&1; echo "build exit: $?" | tee "$RUN_DIR/build.exit"
```

**Seed dữ liệu.** Repo không có script seed người dùng hay dữ liệu mẫu. Chỉ có:

- `SisCom/SisCom/Doc/Totp_Setup.sql`: tạo `sys_totp_setting` và `user_totp`. Không cần chạy tay để đăng nhập thường:
  thiếu bảng thì TOTP được coi là tắt, và `TotpSettingService.SetEnabledAsync` tự tạo bảng khi cần.
- Tài khoản thử: dùng một DB thử đã có sẵn tài khoản trong `userinfo` (hoặc `userinfo_web`).
  Thuật toán `Enc.Encrypt` nằm trong DLL, không có mã nguồn: **không tự chèn hàng vào `userinfo`**.

Dùng DB thử riêng, không dùng DB trong `appsettings.json`. Ghi đè bằng biến môi trường (không in giá trị):

```bash
export ConnectionStrings__Default='<chuỗi kết nối DB thử>'   # [chưa xác minh]
```

> `[chưa xác minh]` `DbManager` nằm trong `SisData.dll` và repo không có `GetConnectionString`. Chưa biết nó có đọc cấu hình
> qua `IConfiguration` (khi đó biến môi trường có tác dụng) hay không. Sau khi khởi động, xác nhận ứng dụng thật sự kết nối
> DB thử, ví dụ qua log của SQL Server, trước khi tin kết quả.

```bash
# 3) Khởi động ở nền, trong nhóm tiến trình riêng để Cleanup chỉ dừng đúng nó.
#    Không dùng "echo $!": setsid có thể fork nên $! không phải PID của ứng dụng.
#    "exec" giữ nguyên PID, nên PID ghi trong app.pid là PID của dotnet và cũng là PGID.
ASPNETCORE_ENVIRONMENT=Development setsid bash -c 'echo $$ > "$0"; exec dotnet run \
    --project SISERPSME/SISERPSME.csproj --no-build --no-launch-profile \
    --urls "http://localhost:1169;https://localhost:1170"' "$RUN_DIR/app.pid" \
    > "$RUN_DIR/server.log" 2>&1 &
```

`--no-launch-profile` tránh `launchBrowser: true` trong `launchSettings.json`; URL lấy từ cùng file đó.
`[chưa chạy với dotnet]` (chặn bởi blocker). Cơ chế PID/PGID/dừng nhóm đã được thử bằng một máy chủ giả trên cổng 1170
(xem `evidence/2026-11-12-run1.md`, mục "Thử cơ chế").

```bash
# 4) Sẵn sàng: https://localhost:1170/ trả 200 và Blazor script có mặt. Thoát sớm nếu tiến trình đã chết.
for i in $(seq 1 60); do
    kill -0 "$(cat "$RUN_DIR/app.pid")" 2>/dev/null || { echo "ứng dụng đã thoát, xem server.log"; break; }
    CODE="$(curl -ksS -m 5 -o "$RUN_DIR/index.html" -w '%{http_code}' https://localhost:1170/ 2>/dev/null)"
    [ "$CODE" = "200" ] && break; sleep 2
done
echo "readiness http: $CODE" | tee "$RUN_DIR/readiness.txt"
curl -ksS -m 10 -o /dev/null -w 'blazor.server.js http: %{http_code}\n' https://localhost:1170/_framework/blazor.server.js | tee -a "$RUN_DIR/readiness.txt"
```

Sẵn sàng khi cả hai dòng là `200`. Cổng mở mà không có `200` là chưa sẵn sàng. `[chưa chạy]`

## Doctor

**Nhận đúng instance của lần chạy này:**

```bash
APP_PID="$(cat "$RUN_DIR/app.pid")"
# a) tiến trình còn sống và đúng lệnh ta đã chạy
tr '\0' ' ' < /proc/"$APP_PID"/cmdline; echo
# b) nhóm tiến trình riêng (setsid): PGID phải bằng PID
ps -o pid,pgid,cmd -g "$APP_PID"
# c) cổng 1170 do chính nhóm này nghe
lsof -nP -iTCP:1170 -sTCP:LISTEN -Fp | sed 's/^p//' | while read -r p; do
    [ "$(ps -o pgid= -p "$p" | tr -d ' ')" = "$APP_PID" ] && echo "OK: cổng 1170 thuộc lần chạy này (pid $p)" || echo "KHÁC: pid $p không thuộc lần chạy này"
done
```

Nếu `/proc/<pid>/cmdline` không tồn tại thì tiến trình đã thoát: đọc `server.log` (lần chạy thật đã gặp lỗi SDK 145 ở đây).

Nếu `doctor.sh` báo cổng 1169/1170 đã bị chiếm trước khi chạy, **dừng**: đó là tiến trình của người khác. Không kill.

**Phát hiện môi trường hỏng:**

| Dấu hiệu | Ý nghĩa |
|---|---|
| `dotnet --version` thoát 145 | Thiếu SDK theo `global.json` |
| `doctor.sh` báo `prebuilt-dll` thiếu | Không build được (`CS0246`) |
| `build.exit` ≠ 0 | Chưa thể chạy; không bỏ qua |
| `server.log` có `Unable to bind` / `address already in use` | Cổng bị chiếm; lần chạy này không phải instance đang nghe |
| Trang tải được nhưng bấm đăng nhập không ra thông báo nào và URL không đổi | Có thể mất kết nối DB: `Onsubbmit` là `async void`, ngoại lệ chỉ được ghi vào `Sqlerr.txt` |
| `index.html` trong `$RUN_DIR` thiếu `blazor.server.js` | Giao diện Blazor chưa dựng đúng |
| Có `NU1603` cho DevExpress | Restore dùng bản khác `24.2.3` → kết quả không đại diện |

## Drive

Repo **không có** test tự động hay script UI sẵn (không có project test, không có `Category=Auth`;
`Microsoft.Playwright` chỉ được khai báo trong `SISERPSME.csproj`, không có mã dùng). Dùng script trong skill này:

```bash
export NODE_PATH="$(npm root -g)"
export VERIFY_USER='<tên tài khoản thử>'
export VERIFY_PASS='<mật khẩu thử>'     # chỉ qua biến môi trường, không đặt vào argv hay log

# Đúng thông tin
node .claude/skills/verify-siserp/scripts/drive-login.js --base https://localhost:1170 --expect success \
    --out "$RUN_DIR/screenshots"; echo "drive success exit: $?" | tee "$RUN_DIR/drive-success.exit"

# Sai thông tin: dùng mật khẩu sai có chủ đích
VERIFY_PASS='sai-mat-khau-co-chu-dich' node .claude/skills/verify-siserp/scripts/drive-login.js \
    --base https://localhost:1170 --expect failure --out "$RUN_DIR/screenshots"; echo "drive failure exit: $?" | tee "$RUN_DIR/drive-failure.exit"
```

Mã thoát của script: `0` đạt kỳ vọng · `1` sai kỳ vọng · `2` không tới được ứng dụng (`BLOCKED`) · `3` cần TOTP.
Chi tiết từng tính năng: [`features/dang-nhap.md`](features/dang-nhap.md).

## Evidence

Đặt tất cả trong `$RUN_DIR` (`${TMPDIR:-/tmp}/verify-siserp/<RUN_ID>/`, ngoài repo để không làm bẩn working tree):

| File | Nội dung |
|---|---|
| `doctor.log`, `doctor.exit` | Kết quả tiền điều kiện |
| `restore.log/.exit`, `build.log/.exit` | Log và mã thoát cài đặt, build |
| `server.log`, `app.pid`, `readiness.txt` | Log máy chủ, PID, kết quả sẵn sàng |
| `drive-success.json`, `drive-failure.json` + `.exit` | Kết quả script: outcome, thông báo, URL cuối, `sessionGuid` |
| `screenshots/*.png` | Ảnh: form đăng nhập và sau khi gửi |
| `git-status.before` / `.after` | So sánh thay đổi file |

**Thành công** chỉ khi tất cả đúng:

1. `doctor.exit` = 0; `restore.exit` = 0; `build.exit` = 0.
2. `readiness.txt` có hai dòng `200`.
3. `drive-success.exit` = 0 và `drive-failure.exit` = 0 (đúng thông tin thì rời `/`; sai thì ở lại `/` kèm
   `Tên đăng nhập hoặc mật khẩu không đúng !`).
4. Không có tệp tracked nào bị đổi, trừ cái do lần chạy này tạo và đã được dọn ở Cleanup.

Có mục nào là `2` (`BLOCKED`) hoặc `3` thì báo "chưa xác minh được" kèm lý do. Không coi là pass.
Lỗi đã có từ trước ghi là **baseline**, không coi là hồi quy.

## Cleanup

Chỉ dọn thứ **lần chạy này** tạo ra. Không `git clean` toàn cục, không kill theo tên tiến trình.

```bash
# 1) Dừng đúng nhóm tiến trình đã khởi động
if [ -f "$RUN_DIR/app.pid" ]; then
    APP_PID="$(cat "$RUN_DIR/app.pid")"
    if [ "$(ps -o pgid= -p "$APP_PID" 2>/dev/null | tr -d ' ')" = "$APP_PID" ]; then
        kill -TERM -- "-$APP_PID"; sleep 3
        kill -0 "$APP_PID" 2>/dev/null && kill -KILL -- "-$APP_PID"
    fi
fi
lsof -nP -iTCP:1169 -iTCP:1170 -sTCP:LISTEN || echo "cổng 1169/1170 đã trống"

# 2) Tệp do lần chạy này tạo hoặc sửa (so với git-status.before của doctor)
git status --short --ignored > "$RUN_DIR/git-status.after"
comm -13 <(sort "$RUN_DIR/git-status.before") <(sort "$RUN_DIR/git-status.after") > "$RUN_DIR/created-by-run.txt"
cat "$RUN_DIR/created-by-run.txt"   # xem lại danh sách rồi mới xóa
```

Với từng dòng trong `created-by-run.txt`: `?? đường/dẫn` hoặc `!! đường/dẫn` → xóa đúng đường dẫn đó (`rm -rf -- 'đường/dẫn'`);
` M đường/dẫn` → `git checkout -- 'đường/dẫn'` (thường là `Sqlerr.txt`, `Totp_debug.txt`). Dự kiến xuất hiện: `Dll/`, `bin/`, `obj/`
của các project, và các file lạ tên có dấu `\` (xem `features/dang-nhap.md`).

**Dữ liệu trong DB thử** (chỉ khi lần chạy đã đăng nhập thành công). Xóa đúng dòng `userlogs` của phiên này,
khóa theo `sessionGuid` ghi trong `drive-success.json` (mã ghi `host = session_guid`):

```sql
DELETE FROM [dbo].[userlogs] WHERE [host] = '<sessionGuid trong drive-success.json>';
```

Không xóa `userinfo`, `userinfo_web` hay `user_totp` của tài khoản có sẵn. Chỉ xóa `user_totp` nếu chính lần chạy này đã
tạo hàng đó (đăng nhập với TOTP bật, `EnsureSecretAsync` chèn hàng).

Cuối cùng xác nhận: `git status --short` giống `git-status.before` và không còn tiến trình do lần chạy này khởi động.
