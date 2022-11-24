var dataTable;

$(document).ready(function () {
    LoadDataTable();
});

function LoadDataTable() {
    dataTable = $("#tableData").dataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [

            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name","width":"15%"},
            /* { "data": "coverType.name","width":"15%"},*/
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-7 btn-group" role="group">
                            <a  href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"> </i>Edit
                            </a>
                        </div>
                    </td>
                    <td>
                        <div class="w-7 btn-group" role="group">
                            <a onClick=Delete('/Admin/Product/Delete/${data}')  class="btn btn-danger mx-2">
                                <i class="bi bi-trash3"></i>  Delete
                            </a>
                        </div>


                            `
                }

            },
        ]

    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {

                    if (data.success) {
                        dataTable.ajax.reload();

                       //$("#tableData").dataTable.ajax.reload();
                      //  $('#tableData').DataTable().ajax.reload(null, false);
                        toastr.success(data.message);
                       // return true;

                    }
                    else {
                        toastr.error(data.message);
                       // return false;
                    }
                }

            })
            
        }
    })
}