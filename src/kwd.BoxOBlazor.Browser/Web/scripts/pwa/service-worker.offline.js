// Caution! Be sure you understand the caveats before publishing an application with
// offline support. See https://aka.ms/blazor-offline-considerations

//build generated app assets.
self.importScripts('./service-worker-assets.js');

//Service worker event registrations.
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;
const offlineAssetsInclude = [/\.dll$/, /\.pdb$/, /\.wasm/, /\.html/, /\.js$/, /\.json$/, /\.css$/, /\.woff$/, /\.png$/, /\.jpe?g$/, /\.gif$/, /\.ico$/];
const offlineAssetsExclude = [/^service-worker\.js$/];

async function onInstall(event) {
    console.info('Service worker: Install');

    let cache = await caches.open(cacheName);

    // Fetch and cache all matching items from the assets manifest
    const assetsRequests = self.assetsManifest.assets
        .filter(asset => offlineAssetsInclude.some(pattern => pattern.test(asset.url)))
        .filter(asset => !offlineAssetsExclude.some(pattern => pattern.test(asset.url)))
        .map(asset => new Request(asset.url, { integrity: asset.hash }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));

    //add Index from host.
    cache.add(new Request("."));
}

async function onActivate(event) {
    console.info('Service worker: Activate');

    // Delete unused caches
    const cacheKeys = await caches.keys();
    await Promise.all(cacheKeys
        .filter(key => key.startsWith(cacheNamePrefix) && key !== cacheName)
        .map(key => caches.delete(key)));
}

async function onFetch(event) {
    let cachedResponse = null;
    if (event.request.method === 'GET') {

        if (event.request.url.toLowerCase().endsWith("/hostconfig")) {
	        return await onlinePreferred(event);
        }

	    // For all navigation requests, try to serve index.html from cache
        // If you need some URLs to be server-rendered, edit the following check to exclude those URLs
        const shouldServeIndexHtml = event.request.mode === 'navigate';

        //const request = shouldServeIndexHtml ? 'index.html' : event.request;
        const request = shouldServeIndexHtml ? new Request(".") : event.request;
        const cache = await caches.open(cacheName);
        cachedResponse = await cache.match(request);
    }

    return cachedResponse || fetch(event.request);
}

async function onlinePreferred(event) {

    const cache = await caches.open(cacheName);

    var response = null;
    var networkError;

    try {
	    response = await fetch(event.request);
    } catch (error) {

        //todo: use Response.error() when can, currently its too experimental.
	    networkError = error;
    }
    
    if (response && response.ok) {
        cache.put(event.request, response.clone());
        return response;
    }

    const cacheResponse = await cache.match(event.request);

    if (cacheResponse) return cacheResponse;

    if (networkError) throw networkError;

    throw "Unable to resolve url";
}
