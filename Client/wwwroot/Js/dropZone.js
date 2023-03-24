export function InitializeFileDropZone(dropZoneElement, inputFile) {
  function onPaste(e) {
    // Set the files property of the input element and raise the change event
    inputFile.files = e.clipboardData.files;
    const event = new Event('change', { bubbles: true });
    inputFile.dispatchEvent(event);
  }

  dropZoneElement.addEventListener('paste', onPaste);

  return {
    dispose: () => {
      dropZoneElement.removeEventListener('paste', onPaste);
    } }
}