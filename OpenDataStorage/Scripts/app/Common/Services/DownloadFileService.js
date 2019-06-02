(function () {
    'use strict';

    window.app.factory('DownloadFileService', DownloadFileService);

    function DownloadFileService() {

        var service = {
            makeLinkElement: makeLinkElement
        };
        return service;

        function makeLinkElement(data, status, headers) {
            headers = headers();

            var filename = null;
            var contentType = headers['content-type'];

            var disposition = headers['content-disposition'];
            if (disposition && disposition.indexOf('attachment') !== -1) {
                var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                var matches = filenameRegex.exec(disposition);
                if (matches != null && matches[1]) {
                    filename = decodeURIComponent(matches[1].replace(/['"]/g, ''));
                }
            }

            var linkElement = document.createElement('a');
            try {
                var blob = new Blob([data], { type: contentType });
                var url = window.URL.createObjectURL(blob);

                linkElement.setAttribute('href', url);
                linkElement.setAttribute("download", filename);

                var clickEvent = new MouseEvent("click", {
                    "view": window,
                    "bubbles": true,
                    "cancelable": false
                });
                linkElement.dispatchEvent(clickEvent);
            }
            catch (ex) {
                throw ex;
            }
        }
    }
})();