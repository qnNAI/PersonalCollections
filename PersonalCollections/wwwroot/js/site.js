const PAGE_SIZE = 10;


function loadUsers(url, page = 1) {
    $.ajax({
        beforeSend: () => $('#loader').show(),
        complete: () => $('#loader').hide(),
        url: `${url}`,
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            "page": page,
            "pageSize": PAGE_SIZE
        }
    }).done(result => {
        refreshUsers(result);
    }).fail(result => {
        let location = result.getResponseHeader('Location');
        window.location.replace(location);
    });;
}

function refreshUsers(result) {
    let body = document.getElementById('tBody');
    body.innerHTML = result;

    let currentPage = document.getElementById('currentPageInput').value;
    let totalPages = document.getElementById('totalPagesInput').value;

    document.getElementById('currentPage').innerText = currentPage;
    document.getElementById('totalPages').innerText = totalPages;

    let prevBtn = document.getElementById('prevBtn');
    let nextBtn = document.getElementById('nextBtn');

    if (+currentPage === 1) {
        if (!prevBtn.classList.contains('disabled')) {
            prevBtn.classList.add('disabled');
        }
    } else {
        if (+totalPages !== 1) {
            prevBtn.classList.remove('disabled');
        }
    }
    if (+currentPage === +totalPages) {
        if (!nextBtn.classList.contains('disabled')) {
            nextBtn.classList.add('disabled');
        }
       
    } else {
        if (+totalPages !== 1) {
            nextBtn.classList.remove('disabled');
        }
    }
}

function loadPrevPage(url) {
    let currentPage = document.getElementById('currentPageInput').value;
    loadUsers(url, +currentPage - 1);
}

function loadNextPage(url) {
    let currentPage = document.getElementById('currentPageInput').value;
    loadUsers(url, +currentPage + 1);
}

function headCheckboxChanged(changed) {
    let checkboxes = document.getElementsByName('userCheckbox');
    checkboxes.forEach(x => x.checked = changed.checked);
}

function manageUsers(url, loadUrl) {
    let selected = getCheckedUsers();
    if (selected.length == 0) return;

    document.getElementById('headCheckbox').checked = false;

    sendSelectedUsers(url, selected)
        .done(result => {
            let currentPage = document.getElementById('currentPageInput').value;
            loadUsers(loadUrl, +currentPage);
        })
        .fail(result => {
            let location = result.getResponseHeader('Location');
            window.location.replace(location);
        });

}


function sendSelectedUsers(url, selected) {
    return $.ajax({
        beforeSend: () => {
            $('#loader').show();
            $('#usersTable').hide();
        },
        complete: () => {
            $('#loader').hide();
            $('#usersTable').show();
        },
        url: `${url}`,
        type: 'POST',
        cache: false,
        async: true,
        dataType: 'html',
        data: { "users": selected }
    });
}

function getCheckedUsers() {
    let checkboxes = document.getElementsByName('userCheckbox');
    let checked = [...checkboxes].filter(x => x.checked === true).map(x => x.id);
    return checked;
}