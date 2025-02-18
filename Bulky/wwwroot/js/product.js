var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({  
        "ajax": {
            "url": "/admin/product/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "title", "width": "20%" },
            { "data": "isbn", "width": "20%" },
            { "data": "price", "width": "8%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "10%" }, 
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-90 btn-group" role="group">
                            <a href="/Admin/Product/Upsert/${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onclick="Delete('/Admin/Product/Delete/${data}')" class="btn btn-danger mx-2">
                                <i class="bi bi-trash"></i> Delete 
                            </a>
                        </div >
                    `;
                },
                "width": "22%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload(); 
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
