window.onload = function () {
    initializePageState();
    bindEventListeners();
};

function initializePageState() {
    setElementValueFromStorage('sortBy');
    setElementValueFromStorage('licenseSort');
    setElementValueFromStorage('searchTerm', "[name='searchTerm']");

    const selectedCategoryId = localStorage.getItem('selectedCategoryId');
    if (selectedCategoryId) {
        const button = document.querySelector(`.category-btn[data-id="${selectedCategoryId}"]`);
        if (button) button.classList.add('selected');
    }
}

function setElementValueFromStorage(storageKey, selector = `#${storageKey}`) {
    const value = localStorage.getItem(storageKey);
    if (value) document.querySelector(selector).value = value;
}

function bindEventListeners() {
    document.querySelectorAll(".category-btn").forEach(button => {
        button.addEventListener("click", handleCategoryButtonClick);
    });

    document.getElementById("sortBy").addEventListener("change", handleSortByChange);
    document.getElementById("licenseSort").addEventListener("change", handleLicenseSortChange);
    document.querySelector(".btn.btn-outline-secondary").addEventListener("click", handleSearchClick);
}

function handleCategoryButtonClick(e) {
    const prevSelected = document.querySelector('.category-btn.selected');
    if (prevSelected) prevSelected.classList.remove('selected');

    if (prevSelected === e.target) {
        localStorage.removeItem('selectedCategoryId');

        const searchTerm = localStorage.getItem('searchTerm');
        if (searchTerm && searchTerm.trim() !== '') {
            return redirectToPage();
        }

        return window.location.href = "/";
    }

    e.target.classList.add('selected');
    localStorage.setItem('selectedCategoryId', e.target.getAttribute('data-id'));
    redirectToPage();
}

function handleSortByChange(e) {
    syncSearchTerm();
    localStorage.setItem('sortBy', e.target.value);
    redirectToPage();
}

function handleLicenseSortChange(e) {
    localStorage.setItem('licenseSort', e.target.value);
    redirectToPage();
}

function handleSearchClick(e) {
    e.preventDefault();
    syncSearchTerm();
    localStorage.setItem('searchTerm', document.querySelector("[name='searchTerm']").value);
    redirectToPage();
}

function syncSearchTerm() {
    const searchTerm = document.querySelector("[name='searchTerm']").value;
    document.getElementById("hiddenSearchTerm").value = searchTerm;
}

function redirectToPage() {
    const params = {
        sortBy: localStorage.getItem('sortBy'),
        categoryId: localStorage.getItem('selectedCategoryId'),
        licenseSort: localStorage.getItem('licenseSort'),
        searchTerm: localStorage.getItem('searchTerm')
    };

    const queryString = Object.entries(params)
        .filter(([_, value]) => value)
        .map(([key, value]) => `${key}=${value}`)
        .join('&');

    window.location.href = `/Home/Index?${queryString}`;
}

function previewImage() {
    const file = document.getElementById("MaterialImage").files[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onloadend = function () {
        document.getElementById("preview").style.display = "block";
        document.getElementById("preview").src = reader.result;
    };
    reader.readAsDataURL(file);
}

var input = document.querySelector('input[name=MaterialTags]');

var tagify = new Tagify(input);
tagify.on('change', function (e) {
    console.log('Tagify value changed:', e.detail.value);
});

$(document).ready(function () {
    $('.new-collection-trigger').click(function () {
        $('.collection-form-card').fadeToggle();
    });

    $('#createCollection').click(function () {
        var collectionName = $('#newCollectionName').val().trim();
        var collectionColor = $('#newCollectionColor').val();

        if (collectionName) {
            var postUrl = $(this).data('url');

            $.ajax({
                url: postUrl,
                type: 'POST',
                data: {
                    title: collectionName,
                    cardColor: collectionColor
                },
                success: function (result) {
                    $('.new-collection-form').fadeOut();
                    location.reload();
                },
                error: function () {
                    alert('Помилка при створенні колекції.');
                }
            });
        } else {
            alert('Будь ласка, введіть назву колекції.');
        }
    });
});

$('.delete-collection').click(function () {
    var collectionId = $(this).data('id');
    var deleteUrl = $(this).data('url') + '/' + collectionId;

    $.ajax({
        url: deleteUrl,
        type: 'POST',
        success: function (result) {
            if (result.success) {
                $('#collectionCard-' + collectionId).remove();
                location.reload();
            } else {
                alert(result.message);
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert('Error occurred while deleting the collection.');
        }
    });
});