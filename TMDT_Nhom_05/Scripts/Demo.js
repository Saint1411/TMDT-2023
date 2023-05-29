$(document).ready(function () {
    $("#search-form").submit(function (e) {
        e.preventDefault(); // Ngăn chặn trình duyệt gửi biểu mẫu
        var keyword = $("#keyword").val(); // Lấy từ khóa tìm kiếm
        var url = "/Admin/ProductAdmin/SearchProducts"; // URL của Action xử lý tìm kiếm sản phẩm

        // Gửi yêu cầu Ajax đến Action xử lý tìm kiếm sản phẩm
        $.ajax({
            url: url,
            type: "GET",
            data: { keyword: keyword },
            success: function (result) {
                $("#product-list").html(result); // Hiển thị danh sách sản phẩm mới
            },
            error: function (xhr, status, error) {
                console.log(error); // Xử lý lỗi khi gửi yêu cầu Ajax
            }
        });
    });
});
