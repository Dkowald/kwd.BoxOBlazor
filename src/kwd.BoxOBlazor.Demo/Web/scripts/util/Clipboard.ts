namespace kwd.BoxOBlazor.Util {

	export namespace Clipboard {
		//based on eg from 
		//https://www.w3schools.com/howto/howto_js_copy_clipboard.asp
		export function copyFromElement(elem: HTMLElement) {

			copyToClipboard(elem.innerText);
		}

		export function copyToClipboard(text: string) {

			let buffer = ensureClipboardBuffer();

			buffer.textContent = text;
			buffer.select();
			buffer.setSelectionRange(0, 99999);

			document.execCommand("copy");

			document.getSelection().removeAllRanges();
		}

		function ensureClipboardBuffer() {
			let buffer = document.getElementById("__internalClipboardBuffer") as HTMLTextAreaElement;
			if (!buffer) {
				buffer = document.createElement("textarea");
				buffer.id = "__internalClipboardBuffer";
				buffer.style.position = "absolute";
				buffer.style.left = "-9999px";
				document.body.append(buffer);
			}

			return buffer;
		}
	}
}