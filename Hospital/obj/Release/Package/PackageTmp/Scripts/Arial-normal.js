(function (jsPDFAPI) {
var font = 'undefined';
var callAddFont = function () {
this.addFileToVFS('Arial-normal.ttf', font);
this.addFont('Arial-normal.ttf', 'Arial', 'normal');
};
jsPDFAPI.events.push(['addFonts', callAddFont])
 })(jsPDF.API);