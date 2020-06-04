var Popup, datatable;
var formDiv = $('<div/>');
var hostName = "https://Localhost:44391";

$(document).ready(function () {
	datatable = $('#datatable').DataTable({
		"ajax": {
			url: hostName +"/Category/GetCategories",
			type: "GET",
			dataType: "json",
		},
		"columns": [
			{ "data": 'NameCategory' },
			{ "data": 'DescriptionCategory' },
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

function PopupForm(url) {
	
	$.get(url)
		.done(function (response) {
			formDiv.html(response);
			Popup = formDiv.dialog({
				autoOpen: true,
				resizable: false,
				width: 400,
				title: 'Fill Category Details',
			}).prev(".ui-dialog-titlebar").addClass("bg-dark text-white card-header text-center");
		});
}

function SubmitForm(form) {
	$.ajax({
		type: 'POST',
		url: form.action,
		data: $(form).serialize(),
		success: function (data) {
			if (data.success) {				
				datatable.ajax.reload();
				$.notify(data.message, {
					globalPosition: 'Top Center',
					className: 'success'
				})
				Popup = formDiv.remove();
			} else {
				Popup = formDiv.remove();
				$.notify(data.message, {
					globalPosition: 'Top right',
					className: 'error'
				})
			}

		}
	});

	return false;
}

function Delete(id) {
	if (confirm('Are you sure you want to delete this Category?')) {
		console.log(hostName + "Category/Delete/" + id)
		$.ajax({
			type: 'POST',
			url: hostName + "/Category/Delete/" + id,
			success: function (data) {
				if (data.success) {
					datatable.ajax.reload();
					$.notify(data.message, {
						globalPosition: 'Top Center',
						className: 'success'
					})
				} else {
					$.notify(data.message, {
						globalPosition: 'Top right',
						className: 'error'
					})
				}
			}
		})
	}
}