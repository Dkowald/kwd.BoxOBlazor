
namespace kwd.BoxOBlazor.Util {
	export function showPrompt(title: string, prompt?: string) {
		return window.prompt(title, prompt || "");
	}
}