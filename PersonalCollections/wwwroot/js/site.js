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
        refreshTable(result);
    }).fail(result => {
        let location = result.getResponseHeader('Location');
        window.location.replace(location);
    });
}

function refreshTable(result) {
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
    if (+totalPages === 0) {
        if (!prevBtn.classList.contains('disabled')) {
            prevBtn.classList.add('disabled');
        }
        if (!nextBtn.classList.contains('disabled')) {
            nextBtn.classList.add('disabled');
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

function addField() {
    let container = document.getElementById('fields');
    let select = document.getElementById('type-select');
    let div = document.createElement('div');
    div.id = `field-container-${counter}`;
    div.classList.add('input-group');
    div.classList.add('mb-3');
    div.innerHTML = `
        <input id="field-name-${counter}" type="text" name="Fields[index].Name" class="form-control" placeholder="Field name" required>
        <span class="input-group-text">${select.options[select.selectedIndex].text}</span>
        <input id="field-type-${counter}" type="hidden" value=${select.value} name="Fields[index].TypeId" />
        <button type="button" class="btn btn-danger" onclick="removeField('field-container-${counter}')">X</button>
    `;
    counter++;
    container.append(div);
}

function addFullField() {
    let container = document.getElementById('fields');
    let select = document.getElementById('type-select');
    let div = document.createElement('div');
    div.id = `field-container-${counter}`;
    div.classList.add('input-group');
    div.classList.add('mb-3');
    div.innerHTML = `
        <input id="field-name-${counter}" type="text" name="Fields[index].Name" class="form-control" placeholder="Field name" required>
        <span class="input-group-text">${select.options[select.selectedIndex].text}</span>
        <input id="field-type-${counter}" type="hidden" value=${select.value} name="Fields[index].FieldType.Id" />
        <button type="button" class="btn btn-danger" onclick="removeField('field-container-${counter}')">X</button>
    `;
    counter++;
    container.append(div);
}

function removeField(id) {
    let field = document.getElementById(id);
    field.remove();
}

function arrangeTypes() {
    let index = 0;
    let fields = document.querySelectorAll('[id^="field-container"]');

    for (let field of fields) {
        let postfix = field.id.substring(16);

        let fieldName = document.getElementById(`field-name-${postfix}`);
        fieldName.name = fieldName.name.replace('index', index);

        let fieldType = document.getElementById(`field-type-${postfix}`)
        fieldType.name = fieldType.name.replace('index', index);

        index++;
    }
}

function arrangeTypesStartWith(index) {
    let fields = document.querySelectorAll('[id^="field-container"]');

    for (let field of fields) {
        let postfix = field.id.substring(16);

        let fieldName = document.getElementById(`field-name-${postfix}`);
        fieldName.name = fieldName.name.replace('index', index);

        let fieldType = document.getElementById(`field-type-${postfix}`)
        fieldType.name = fieldType.name.replace('index', index);

        index++;
    }
}

function setupFileInput() {
    let dropEl = document.getElementById('drop-area');
    let fileInput = document.getElementById('file-input');
    
    dropEl.ondragover = (evt) => { evt.preventDefault(); };

    dropEl.ondragenter = (evt) => { 
        evt.preventDefault();
        dropEl.classList.add('bg-primary-subtle');
    };

    dropEl.ondragleave = (evt) => {
        dropEl.classList.remove('bg-primary-subtle');
    };

    dropEl.ondrop = (evt) => {
        evt.preventDefault();
        fileInput.files = evt.dataTransfer.files;
        let files = [...evt.dataTransfer.files]
        previewFile(files[0]);
    };

    fileInput.onchange = (evt) => {
        let files = [...fileInput.files]
        previewFile(files[0]);
    };
}

function previewFile(file) {
    let image = document.getElementById('image-preview');
    let imageText = document.getElementById('drop-area-text');

    if (file === undefined) {
        image.src = '';
        image.classList.add('d-none');
        imageText.classList.remove('d-none');
        return;
    }

    let reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = function () {
        image.classList.remove('d-none');
        imageText.classList.add('d-none');

        image.src = reader.result;
    }
}

function removeCollection(url, id) {
    $.ajax({
        url: `${url}`,
        type: 'POST',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            "collectionId": id
        }
    }).done(() => {
        document.getElementById(id).remove();
    }).fail(() => {
        showWarning('Failed to remove collection!');
    });

}

function removeItem(url, id) {
    $.ajax({
        url: `${url}`,
        type: 'POST',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            "itemId": id
        }
    }).done(() => {
        document.getElementById(id).remove();
    }).fail(() => {
        showWarning('Failed to remove item!');
    });

}

function showWarning(message) {
    const toastLive = document.getElementById('liveToast')

    const toastBootstrap = bootstrap.Toast.getOrCreateInstance(toastLive);
    document.getElementById('toastBody').innerText = message;
    toastBootstrap.show()
}

function setTags(formId) {
    let tags = document.getElementsByClassName('tag');
    let form = document.getElementById(formId);
    let counter = 0;
    for (let tag of tags) {
        let tagInput = document.createElement('input');
        tagInput.setAttribute('type', 'hidden');
        tagInput.setAttribute('name', `Tags[${counter}].Name`);
        tagInput.value = tag.innerText;

        form.append(tagInput);

        counter++;
    }

}

function loadItems(url, collectionId, page = 1) {
    let order = document.getElementById('order-select').value;
    let filter = document.getElementById('filter').value;
    let dateEntries = collectDateFields();
    let data = {
        "page": page,
        "pageSize": PAGE_SIZE,
        "order": order,
        "filter": filter,
        "collectionId": collectionId
    }

    let dateCounter = 0;
    dateEntries.forEach(x => {
        data[`DateEntries[${dateCounter}].Id`] = x.id;
        data[`DateEntries[${dateCounter}].Value`] = x.value;
        dateCounter++;
    });
    
    $.ajax({
        beforeSend: () => $('#loader').show(),
        complete: () => $('#loader').hide(),
        url: `${url}`,
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data: data
    }).done(result => {
        refreshTable(result);
    }).fail(result => {
        let location = result.getResponseHeader('Location');
        window.location.replace(location);
    });

}

function collectDateFields() {
    let dates = document.querySelectorAll('[id^="date-"]');
    let dateEntries = [...dates].filter(x => x.value !== "").map(x => ({
        id: x.id.substring(5),
        value: x.value
    }));

    return dateEntries;
}

function loadPrevItemsPage(url, collectionId) {
    let currentPage = document.getElementById('currentPageInput').value;
    loadItems(url, collectionId, +currentPage - 1);
}

function loadNextItemsPage(url, collectionId) {
    let currentPage = document.getElementById('currentPageInput').value;
    loadItems(url, collectionId, +currentPage + 1);
}

function setTheme(theme) {
    localStorage.setItem('theme', theme);

    let html = document.getElementById('html-el');
    html.setAttribute('data-bs-theme', theme);
}

function loadTheme() {
    document.addEventListener('DOMContentLoaded', function () {
        let theme = localStorage.getItem('theme');
        if (theme) {
            setTheme(theme);
        };

    }, false);
    
}

function toggleLike(itemId) {
    let btn = document.getElementById('like-btn');
    let action;
    if (btn.getAttribute('data-liked') === 'like') {
        action = 'dislike';
    }
    else {
        action = 'like';
    }

    hubConnection.invoke("Like", itemId, action);
}

function setupItemHub() {
    hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("/item")
        .build();
    let itemId = document.getElementById('item-id').value;

    setupHubEndpoints();
    hubConnection.start()
        .then(() => {
            hubConnection.invoke("JoinItemGroup", itemId);
        });
}

function setupHubEndpoints() {
    hubConnection.on('LikeSuccess', () => {
        let btn = document.getElementById('like-btn');
        if (btn.getAttribute('data-liked') === 'like') {
            btn.removeAttribute('data-liked');
        }
        else {
            btn.setAttribute('data-liked', 'like');
        }

        let icons = document.getElementsByClassName('like-icon');
        for (let icon of icons) {
            if (icon.classList.contains('d-none')) {
                icon.classList.remove('d-none');
            }
            else {
                icon.classList.add('d-none');
            }
        }
    });

    hubConnection.on('LikesUpdate', (likes) => {
        document.getElementById('likes').innerText = likes;
    });

    hubConnection.on('NewComment', (comment, date) => {
        let commentEl = htmlToElement(`
            <div class="col-12 col-lg-6 border-bottom pb-1 mb-3">
            <a class="link-underline link-underline-opacity-0" href="/Collection/CollectionsManagement?userId=${comment.userId}">${comment.author.userName}</a>
            <p class="text-muted small mb-2">
                ${date}
            </p>
            <span>
                ${comment.text}
            </span>
        </div>`);
        let commentsContainer = document.getElementById('comments');
        commentsContainer.prepend(commentEl);
    });

    hubConnection.on('CommentFailed', (message) => {
        showWarning(message);
    });
}

function htmlToElement(html) {
    var template = document.createElement('template');
    html = html.trim();
    template.innerHTML = html;
    return template.content.firstChild;
}

function sendComment() {
    let textArea = document.getElementById('comment-text');
    let text = textArea.value;
    let itemId = document.getElementById('item-id').value;
    debugger;
    hubConnection.invoke('Comment', {
        text,
        itemId
    });

    textArea.value = '';
}

function loadComments() {
    let commentsContainer = document.getElementById('comments');
    let commentsCount = [...commentsContainer.children].length;
    let itemId = document.getElementById('item-id').value;

    $.ajax({
        beforeSend: () => $('#loader').show(),
        complete: () => $('#loader').hide(),
        url: 'Comments',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            itemId,
            skip: commentsCount,
            pageSize: 5
        }
    }).done(result => {
        if (result === '\r\n') {
            window.removeEventListener("scroll", handleScroll);
        } else {
            commentsContainer.innerHTML += result;
        }
    });

}

function addInfiniteScroll() {
    window.addEventListener("scroll", handleScroll);
}

let scrollTimer;
function handleScroll() {
    clearTimeout(scrollTimer);
    scrollTimer = setTimeout(function () {
        const endOfPage = window.innerHeight + window.pageYOffset >= document.body.offsetHeight;
        if (endOfPage) {
            loadComments();
        }
    }, 500);
} 

function searchShowSpinner() {
    let spinner = document.getElementById('srch-spinner');
    spinner.classList.remove('d-none');
}

function addSearchItemsInfiniteScroll() {
    window.addEventListener("scroll", handleSearchInfiniteScroll);
}

let searchScrollTimer;
function handleSearchInfiniteScroll() {
    clearTimeout(scrollTimer);
    scrollTimer = setTimeout(function () {
        const endOfPage = window.innerHeight + window.pageYOffset >= document.body.offsetHeight;
        if (endOfPage) {
            let itemsPane = document.getElementById('items-pane');

            if (itemsPane.classList.contains('active')) {
                if (isItemsRemaining) {
                    searchItems();
                }
            }
            else {
                if (isCollectionsRemaining) {
                    searchCollections();
                }
            }
        }
    }, 500);
}

let itemsPage = 2;
let isItemsRemaining = true;
function searchItems() {
    let term = document.getElementById('term-hidden').value;
    let itemsContainer = document.getElementById('items-pane');

    if (term === undefined || term === '') {
        return;
    }

    $.ajax({
        beforeSend: () => $('#loader').show(),
        complete: () => $('#loader').hide(),
        url: 'SearchItems',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            term,
            page: itemsPage,
            pageSize: 10
        }
    }).done(result => {
        if (result === '\r\n') {
            isItemsRemaining = false;
        } else {
            itemsContainer.innerHTML += result;
            itemsPage++;
        }
    });
}

let collectionsPage = 2;
let isCollectionsRemaining = true;
function searchCollections() {
    let term = document.getElementById('term-hidden').value;
    let collectionsContainer = document.getElementById('collections-pane');

    if (term === undefined || term === '') {
        return;
    }

    $.ajax({
        beforeSend: () => $('#loader').show(),
        complete: () => $('#loader').hide(),
        url: '/Collection/SearchCollections',
        type: 'GET',
        cache: false,
        async: true,
        dataType: 'html',
        data: {
            term,
            page: collectionsPage,
            pageSize: 10
        }
    }).done(result => {
        if (result === '\r\n') {
            isCollectionsRemaining = false;
        } else {
            collectionsContainer.innerHTML += result;
            collectionsPage++;
        }
    });
}