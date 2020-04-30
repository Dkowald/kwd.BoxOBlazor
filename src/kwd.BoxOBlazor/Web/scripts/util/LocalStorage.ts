namespace kwd.BoxOBlazor.Util {

	export function HookStorageEvent(objRef) {

		window.addEventListener("storage", async (evt) => {

			//sync version raises error? use the async version.
			await objRef.invokeMethodAsync("HandleStorageEvent", evt);
		});
	}

	export function hasLocalStorage() { return !!(window.localStorage); }

	export function getLength() {return window.localStorage.length;}
}