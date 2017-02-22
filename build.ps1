# Bootstrap paket
.paket/paket.bootstrapper.exe
if ($LASTEXITCODE -ne 0) {
    exit 1
}

.paket/paket.exe restore
if ($LASTEXITCODE -ne 0) {
    exit 2
}

packages/FAKE/tools/FAKE.exe @0 --fsiargs build.fsx
if ($LASTEXITCODE -ne 0) {
    exit 3
}