# Overview
Tool to update file(s) for a published blazor web assembly project.

This tool scans service-worker-assets.js, updating the hash value for 
all the referenced files.

You can also use this to update the base element in the default html file.

## Install
This is a dotnet cli tool, use following to install:

> dotnet tool install BlazorStamp

See also: [manage dotnet tools](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools).

## Usage
CLI help : __dotnet blazorstamp ?__

Option can be configured via appsettings.config, environment or arguments.

|Option|Description|
|---|---|
|**TargetPath**| Path to wasm files, defaults to current directory (./)|
|**DefaultFile**| Html default file, defaults to _index.html_ |
|**BaseUrl**|  (optional) Replacement href for base element in default file.|

e.g. Basic usage, just change the blazor wasm files as desired, 
then run 
> dotent blazorstamp

in the same directory.
This will update the service-worker-assets.js file to reflect changes.

e.g.
Replace the base element href with _/wasm/_
> dotnet blazorstamp base=/wasm/

This will replace the base element href value with /wasm/.
Then re-calculate all hash's in service-worker-assets.js.
Finally re-creating the compressed version(s) of service-worker-assets.js
