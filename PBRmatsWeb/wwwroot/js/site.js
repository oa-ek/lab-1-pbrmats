window.onload = function () {
    const selectedCategoryId = localStorage.getItem('selectedCategoryId');
    const sortBy = localStorage.getItem('sortBy');
    const licenseSort = localStorage.getItem('licenseSort');

    if (sortBy) {
        document.getElementById('sortBy').value = sortBy;
    }

    if (licenseSort) {
        document.getElementById('licenseSort').value = licenseSort;
    }

    if (selectedCategoryId) {
        const button = document.querySelector(`.category-btn[data-id="${selectedCategoryId}"]`);
        if (button) {
            button.classList.add('selected');
        }
    }
};

document.querySelectorAll(".category-btn").forEach(button => {
    button.addEventListener("click", function (e) {
        const prevSelected = document.querySelector('.category-btn.selected');
        if (prevSelected) {
            prevSelected.classList.remove('selected');
        }

        if (prevSelected === e.target) {
            localStorage.removeItem('selectedCategoryId');
            window.location.href = "/";
        } else {
            e.target.classList.add('selected');
            localStorage.setItem('selectedCategoryId', e.target.getAttribute('data-id'));
            redirectToPage();
        }
    });
});

document.getElementById("sortBy").addEventListener("change", function (e) {
    localStorage.setItem('sortBy', e.target.value);
    redirectToPage();
});

document.getElementById("licenseSort").addEventListener("change", function (e) {
    localStorage.setItem('licenseSort', e.target.value);
    redirectToPage();
});

function redirectToPage() {
    const sortBy = localStorage.getItem('sortBy') || '';
    const selectedCategoryId = localStorage.getItem('selectedCategoryId') || '';
    const licenseSort = localStorage.getItem('licenseSort') || '';

    let url = `/Home/Index?sortBy=${sortBy}`;

    if (selectedCategoryId) {
        url += `&categoryId=${selectedCategoryId}`;
    }

    if (licenseSort) {
        url += `&licenseSort=${licenseSort}`;
    }

    window.location.href = url;
}

function previewImage() {
    var file = document.getElementById("MaterialImage").files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        document.getElementById("preview").style.display = "block";
        document.getElementById("preview").src = reader.result;
    }

    if (file) {
        reader.readAsDataURL(file);
    }
}


/*
<script>
    window.onload = function () {
        const selectedCategoryId = localStorage.getItem('selectedCategoryId');
    const sortBy = localStorage.getItem('sortBy');

    if (sortBy) {
            const select = document.getElementById('sortBy');
    select.value = sortBy;
        }

    if (selectedCategoryId) {
            const button = document.querySelector(`.category-btn[data-id="${selectedCategoryId}"]`);
    if (button) {
        button.classList.add('selected');
            }
        }
    };

    document.querySelectorAll(".category-btn").forEach(button => {
        button.addEventListener("click", function (e) {
            const prevSelected = document.querySelector('.category-btn.selected');
            if (prevSelected) {
                prevSelected.classList.remove('selected');
            }

            if (prevSelected === e.target) {
                localStorage.removeItem('selectedCategoryId');
                window.location.href = "/";
            } else {
                e.target.classList.add('selected');
                localStorage.setItem('selectedCategoryId', e.target.getAttribute('data-id'));
                submitCategoryForm();
            }
        });
    });

    document.getElementById("sortBy").addEventListener("change", function (e) {
        localStorage.setItem('sortBy', e.target.value);
    submitSortByForm();
    });

    function submitCategoryForm() {
        const sortBy = localStorage.getItem('sortBy') || ''; // Use the saved value or default to an empty string
    const selectedCategoryId = localStorage.getItem('selectedCategoryId');

    if (selectedCategoryId) {
        window.location.href = `/Home/Index?categoryId=${selectedCategoryId}&sortBy=${sortBy}`;
        } else {
        window.location.href = `/Home/Index?sortBy=${sortBy}`;
        }
    }

    function submitSortByForm() {
        const selectedCategoryId = localStorage.getItem('selectedCategoryId') || '';
    const sortByValue = document.getElementById("sortBy").value;
    window.location.href = `/Home/Index?categoryId=${selectedCategoryId}&sortBy=${sortByValue}`;
    }
</script>*/