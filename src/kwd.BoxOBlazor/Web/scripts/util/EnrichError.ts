namespace kwd.BoxOBlazor.Util {

	/** Update message to have richer (JSON) data. */
	export function EnrichError(e: any): Error {

		if (e instanceof DOMException) {
			const data = JSON.stringify({
				name: "DOMException",
				domErrorName: e.name,
				message: e.message
			});
			var mapped = new Error(data);
			mapped.stack = (e as any).stack;
			mapped.name = "DOMException";
			
			return mapped;
		}

		if (e instanceof Error) {
			const data = JSON.stringify({
				name: e.name,
				message: e.message
			});

			var mapped = new Error(data);
			mapped.name = e.name;
			mapped.stack = (data as any).stack;
			return mapped;
		}

		return e;
	}
}