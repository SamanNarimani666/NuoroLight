function open_waiting(selector = 'body') {
    $(selector).waitMe({
        effect: 'facebook',
        text: 'لطفا صبر کنید ...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
    setTimeout(function () {
        location.reload();
    }, 3000);

}

function close_waiting(selector = 'body') {
    $(selector).waitMe('hide');
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});



$('#productCount').on('change',
    function (e) {
        var numberOfProduct = parseInt(e.target.value, 0);
        $('#add_product_to_cart_Count').val(numberOfProduct);
    });



const cookieName = "cart-items";

function addToCart(productId) {

    let colorId = parseInt($("#productColorId").val());
    const count = parseInt($("#productCount").val());

    if (isNaN(colorId)) {
        colorId = null;
    }

    $.post("/addToCart",
        {
            productId: productId,
            productColorId: colorId,
            count: count
        },
        function (result) {
            if (result.status === 'Success') {

                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                });

                Toast.fire({
                    icon: 'success',
                    title: 'به سبد خرید اضافه شد'
                });
                updateCart();
            }

        });
}


function updateCart() {
    let products = $.cookie(cookieName);
    products = JSON.parse(products);
    let countOfProduct = products.length;
    $(".cart-count").text(countOfProduct);
    $(".header-cart-info-count").text(countOfProduct + 'کالا');
    let cartItemUl = $(".header-basket-list");
    cartItemUl.html('');

    if (countOfProduct != 0) {
        var productInfo = function () {
            var tmp = null;
            $.ajax({
                'async': false,
                'type': "GET",
                'global': false,
                'dataType': 'html',
                'url': "/GetBasketDetail",
                'success': function (data) {
                    tmp = data;
                }
            });

            return JSON.parse(tmp);
        }();

        productInfo.data.forEach(x => {
            const product = `
                                            <li class="js-mini-cart-item">
                                                <a href="#" class="header-basket-list-item">
                                                    <div class="header-basket-list-item-image">
                                                        <img src="${x.picture}"
                                                             alt="img-slider">
                                                    </div>
                                                    <div class="header-basket-list-item-content">
                                                        <h1 class="header-basket-list-item-title">
                                                            ${x.title}
                                                        </h1>
                                                        <span class="header-basket-list-item-shipping-type">
                                                            ${x.priceLetters} تومان
                                                        </span>
                                                        <div class="header-basket-list-item-footer">
                                                            <div class="header-basket-list-item-props">
                                                                <span class="header-basket-list-item-props-item">
                                                                    ${x.count}
                                                                    عدد
                                                                </span>
                                                                <span class="header-basket-list-item-props-item">
                                                                    <span class="header-basket-list-item-color-badge"
                                                                          style="background: ${x.colorCode}">
                                                                    </span>
                                                                    ${x.colorName}
                                                                </span>
                                                                <span class="header-basket-list-item-remove" onclick="removeFromCart(${x.productId},${x.productColorId})">
                                                                    <i class="mdi mdi-delete"></i>
                                                                </span>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </li>`;

            cartItemUl.append(product);
        });
        const sum = productInfo.data.reduce((accumulator, object) => {


            return accumulator + (object.productPriceDigit * object.count);
        }, 0);
        $("#sumCartItem").text(sum);
        $(".header-cart-info-total-amount-number").text(sum);
    }
    else {
        cartItemUl.append('<span>سبد خرید خالی است</span>');
        $("#sumCartItem").text(0);
        $(".header-cart-info-total-amount-number").text(0);
    }
}


function removeFromCart(productId, colorId) {

    Swal.fire({
        text: "آیا مطمئن هستید حذف شود؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر'
    }).then((result) => {
        if (result.isConfirmed) {
            let products = $.cookie(cookieName);
            products = JSON.parse(products);
            const itemRemove = products.findIndex(x => x.productId === productId && x.productColorId === colorId);
            products.splice(itemRemove, 1);
            $.cookie(cookieName, JSON.stringify(products), { expires: 2, path: "/" });
            updateCart();
            Swal.fire({
                title: 'حذف شد!',
                confirmButtonText: 'باشه',
                icon: 'success'
            })
        }
    })
}

function removeFormCartForPage(productId, colorId) {

    Swal.fire({
        text: "آیا مطمئن هستید حذف شود؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر'
    }).then((result) => {
        if (result.isConfirmed) {

            $.get("/RemoveFromCart/" + productId + "/" + colorId);
            Swal.fire({
                title: 'حذف شد!',
                confirmButtonText: 'باشه',
                icon: 'success'
            })
            $("#productinfo-" + productId + "-" + colorId).hide(2000);
            open_waiting();
        }
    })
}

function changeCountItemCart(productId, colorId, count) {
    $.post("/ChangeBasketCount",
        {
            productId: productId,
            productColorId: colorId,
            count: count
        },
        function (result) {
            if (result.data.count != 0) {

                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 2000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                });

                Toast.fire({
                    icon: 'success',
                    title: result.message
                });

                setTimeout(function () {
                    open_waiting();
                }, 2500);

            } else if (result.data.count == 0) {
                open_waiting();
            }
        });
    updateCart();
}

var searchHistoryList = JSON.parse(localStorage.getItem("searchHistory"));

$("#searchForm").submit(() => {
    debugger;
    var q = $("#searhHistory").val();

    if (q !== undefined || q !== "") {
        if (searchHistoryList == null) {
            searchHistoryList = [];
        }
        if (!searchHistoryList.includes(q)) {
            searchHistoryList.push(q);
        }
        localStorage.setItem("searchHistory", JSON.stringify(searchHistoryList));
    }
    loadSearchHistory();
});

function loadSearchHistory() {
    var ulSearchResult = $("#search-result-most-view");
    searchHistoryList.forEach(x => {
        var search = `<li><a href="/search?q=${x}">${x} <i class="fa fa-angle-left"></i></a></li>`;

        ulSearchResult.append(search);
    });
}

function clearSearchHistory() {
    localStorage.removeItem("searchHistory");
    open_waiting();
}

function AddToProductList(id) {
    $.post("/UserPanel/AddToFavoriteList",
        {
            productId: id
        },
        function (result) {
            if (result.status === 'Success') {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                });

                Toast.fire({
                    icon: 'success',
                    title: result.message
                });

                setTimeout(function () {
                    location.reload();
                }, 3000);
            }

        });
}

function CheckProductInUserFavoriteList(id) {
    $.post("/UserPanel/CheckProductInUserFavoriteList",
        {
            productId: id
        },
        function (result) {
            if (result.status === 'Success') {
                console.log(result.data);
                if (result.data == true) {
                    $("#productFavorite").addClass("btn-option-favorites");

                }
            }
        });
}

function RemoveProductFromFavoriteList(id) {
    Swal.fire({
        text: "آیا مطمئن هستید حذف شود؟",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'بله',
        cancelButtonText: 'خیر'
    }).then((result) => {
        if (result.isConfirmed) {

            $.post("/UserPanel/RemoveProductFromFavoriteList",
                {
                    productId: id
                },
                function (result) {
                    if (result.status === 'Success') {
                        if (result.data == true) {
                        }
                    }
                    open_waiting();

                });
        }
    })
}