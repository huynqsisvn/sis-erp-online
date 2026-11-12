#!/usr/bin/env bash
# Doctor cho SIS ERP Online: chi DOC, khong sua repo, khong khoi dong dich vu.
# Ma thoat: 0 = san sang chay; 1 = co it nhat mot blocker.
# Dung: bash .claude/skills/verify-siserp/scripts/doctor.sh [RUN_DIR]
set -u

REPO_ROOT="$(git rev-parse --show-toplevel)"
cd "$REPO_ROOT" || exit 2

RUN_DIR="${1:-${RUN_DIR:-${TMPDIR:-/tmp}/verify-siserp/doctor-$(date -u +%Y%m%dT%H%M%SZ)}}"
mkdir -p "$RUN_DIR"
LOG="$RUN_DIR/doctor.log"
: > "$LOG"

FAILS=0
report() { # report PASS|FAIL|INFO <ten> <chi tiet>
    printf '%-4s %-22s %s\n' "$1" "$2" "$3" | tee -a "$LOG"
    [ "$1" = "FAIL" ] && FAILS=$((FAILS + 1))
}

# 1) Phien ban repo
report INFO commit "$(git rev-parse HEAD) nhanh=$(git branch --show-current)"

# 2) .NET SDK theo global.json (dotnet that bai voi ma 145 neu khong co SDK phu hop)
WANT_SDK="$(sed -n 's/.*"version" *: *"\([^"]*\)".*/\1/p' global.json | head -1)"
if command -v dotnet >/dev/null 2>&1; then
    HAVE_SDK="$(dotnet --version 2>"$RUN_DIR/dotnet-version.err")"; RC=$?
    if [ "$RC" -eq 0 ]; then
        report PASS dotnet-sdk "global.json yeu cau $WANT_SDK, dang dung $HAVE_SDK"
    else
        report FAIL dotnet-sdk "global.json yeu cau $WANT_SDK, 'dotnet --version' thoat $RC (SDK da cai: $(dotnet --list-sdks | tr '\n' ';'))"
    fi
else
    report FAIL dotnet-sdk "khong co lenh dotnet (can SDK $WANT_SDK)"
fi

# 3) DLL dung san: lay danh sach tu <HintPath> trong .csproj, khong doan
MISSING=""
while IFS= read -r dll; do
    [ -f "Dll/Debug/net8.0/$dll" ] || MISSING="$MISSING $dll"
done < <(grep -rhoE 'Dll\\Debug\\net8\.0\\[^<]+' --include=*.csproj . | sed 's#.*\\##' | sort -u)
if [ -z "$MISSING" ]; then
    report PASS prebuilt-dll "du DLL trong Dll/Debug/net8.0"
else
    report FAIL prebuilt-dll "thieu trong Dll/Debug/net8.0:$MISSING"
fi
for d in SisData SisFunc SisSm; do
    grep -rIlE "namespace $d\b" --include=*.cs . >/dev/null 2>&1 || report INFO "src-$d" "khong co ma nguon trong repo (chi co DLL)"
done

# 4) NuGet (restore goi DevExpress/Otp.NET/... tu nuget.org; repo khong co nuget.config)
CODE="$(curl -sS -m 15 -o /dev/null -w '%{http_code}' https://api.nuget.org/v3/index.json 2>/dev/null)"
if [ "$CODE" = "200" ]; then report PASS nuget-org "api.nuget.org HTTP $CODE"; else report FAIL nuget-org "api.nuget.org HTTP ${CODE:-000}"; fi

# 5) SQL Server: chi lay TEN MAY tu cau hinh, khong in mat khau
CONN="${ConnectionStrings__Default:-$(sed -n 's/.*"Default" *: *"\(Server=[^"]*\)".*/\1/p' SISERPSME/appsettings.json | head -1)}"
SQLHOST="$(printf '%s' "$CONN" | sed -n 's/.*Server=\([^;\\,]*\).*/\1/Ip' | head -1)"
if [ -z "$SQLHOST" ]; then
    report FAIL sql-server "khong doc duoc Server= tu cau hinh"
elif getent hosts "$SQLHOST" >/dev/null 2>&1 || [ "$SQLHOST" = "localhost" ] || [ "$SQLHOST" = "." ]; then
    if timeout 5 bash -c "exec 3<>/dev/tcp/$SQLHOST/1433" 2>/dev/null; then
        report PASS sql-server "may $SQLHOST phan giai duoc, TCP 1433 mo"
    else
        report FAIL sql-server "may $SQLHOST phan giai duoc nhung TCP 1433 khong mo (instance co ten dung cong dong; xac nhan lai)"
    fi
else
    report FAIL sql-server "khong phan giai duoc ten may '$SQLHOST'"
fi

# 6) Cong ung dung (launchSettings.json: http 1169, https 1170)
for p in 1169 1170; do
    if [ -n "$(lsof -nP -iTCP:$p -sTCP:LISTEN -t 2>/dev/null)" ]; then
        report FAIL "port-$p" "da co tien trinh khac nghe cong $p: $(lsof -nP -iTCP:$p -sTCP:LISTEN -t | tr '\n' ' ')"
    else
        report PASS "port-$p" "cong trong"
    fi
done

# 7) Playwright (node) de lai xe UI
if NODE_PATH="$(npm root -g 2>/dev/null)" node -e "require('playwright')" 2>/dev/null; then
    report PASS playwright "node playwright $(NODE_PATH="$(npm root -g)" node -e "console.log(require('playwright/package.json').version)")"
else
    report FAIL playwright "khong require duoc 'playwright' (npm root -g=$(npm root -g 2>/dev/null))"
fi

# 8) Trang thai working tree luc bat dau (de Cleanup so sanh)
git status --short --ignored > "$RUN_DIR/git-status.before"
report INFO git-status "$(wc -l < "$RUN_DIR/git-status.before") dong (luu $RUN_DIR/git-status.before)"

echo "doctor ket thuc: $FAILS blocker" | tee -a "$LOG"
echo "RUN_DIR=$RUN_DIR"
[ "$FAILS" -eq 0 ]
