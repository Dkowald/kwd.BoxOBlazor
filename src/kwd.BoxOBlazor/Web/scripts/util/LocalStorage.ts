/// <reference path="./EnrichError.ts"/>

namespace kwd.BoxOBlazor.Util {

	export function HookStorageEvent(objRef) {

		window.addEventListener("storage", async (evt:StorageEvent) => {

			var payload = {
				Key : evt.key,
				NewValue : evt.newValue,
				OldValue : evt.oldValue,
				StorageArea : evt.storageArea,
				Url : evt.url
			};

			//sync version raises error? use the async version.
			await objRef.invokeMethodAsync("HandleStorageEvent", payload);
		});
	}

	//Credits: https://developer.mozilla.org/en-US/docs/Web/API/Web_Storage_API/Using_the_Web_Storage_API
	function storageAvailable(type) {
		var storage;
		try {
			storage = window[type];
			var x = '__storage_test__';
			storage.setItem(x, x);
			storage.removeItem(x);
			return true;
		}
		catch (e) {
			return e instanceof DOMException && (
					// everything except Firefox
					e.code === 22 ||
						// Firefox
						e.code === 1014 ||
						// test name field too, because code might not be present
						// everything except Firefox
						e.name === 'QuotaExceededError' ||
						// Firefox
						e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
				// acknowledge QuotaExceededError only if there's something already stored
				(storage && storage.length !== 0);
		}
	}

	let _hasLocalStorage = null;
	export function hasLocalStorage() {
		if (_hasLocalStorage == null)
			_hasLocalStorage = storageAvailable("localStorage");
		return _hasLocalStorage;
	}

	let _hasSessionStorage;
	export function hasSessionStorage() {
		if (_hasSessionStorage == null)
			_hasSessionStorage = storageAvailable("sessionStorage");

		return _hasSessionStorage;
	}
	
	export namespace LocalStorage {

		/**Request the length, using method for interop.  */
		export function getLength() { return window.localStorage.length; }

		/** Set item in local storage, errors are enriched */
		export function setItem(key: string, value: string) {

			try {
				window.localStorage.setItem(key, value);
			} catch (e) {
				throw EnrichError(e);
			}
		}
	}
}