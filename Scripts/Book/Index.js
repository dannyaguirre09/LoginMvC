var Popup, datatable;
var formDiv = $('<div/>');
var hostName = "https://Localhost:44391";

$(document).ready(function () {
	datatable = $('#datatable').DataTable({
		"ajax": {
			url: hostName + "/Book/GetBooks",
			type: "GET",
			dataType: "json",
		},
		"columns": [
			{ "data": 'NameBooK' },
			{ "data": 'DescriptionBook' },
			{ "data": "NameCategory" },
			{
				"data": 'IdCategory', 'render': function (data) {
					return `<a class = 'text-white btn btn-primary' style = 'margin-left: 10px' 
						 onclick = PopupForm('${hostName}/Category/AddOrEdit/${data}')> Edit<a/> 
					 <a class = ' text-white btn btn-secondary' onclick = 'Delete(${data})' >Delete</a> `

				},
				"orderable": false,
				"searcheable": false,
				"width": "150px"
			}
		],
		'language': {
			'emptytable': 'No data found, please <b>Click on Add New</b> Button'
		},
		lengthMenu: [5, 10, 20, 50, 100]
	})
})