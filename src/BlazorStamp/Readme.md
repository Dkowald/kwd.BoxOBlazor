# Overview
Tool to update file(s) in a published blazor web assembly project.

1. Provide a custom urlBase to update the default html document (index.html).

2. Updates service-worker-assets.js with new hash's.

## Usage
A simple .NET tool driven by config.
Use appsettings.config or environment or command line.

If provided; **UrlBase** is used to set the base elment in the default html document.

If service-worker-assets.js file exists; hashes are re-calculated and saved.


