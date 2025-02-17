let allProducts = [];

// Sayfa ilk yüklendiðinde tüm ürünleri al
async function loadAllProducts() {
    const apiUrl = `http://127.0.0.1:5000/search?q=`;  // Parametre boþ býrakýlýrsa tüm ürünleri alýr

    try {
        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // HTTP yanýtýný kontrol et
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.statusText}`);
        }

        allProducts = await response.json(); // API'den tüm ürünleri al
        updateTable(allProducts); // Tabloyu tüm ürünlerle güncelle
    } catch (error) {
        console.error('Fetch error:', error.message);
        alert('Ürünleri alýrken bir hata oluþtu. Lütfen tekrar deneyin.');
    }
}

// Sunucudan ürün arama
async function searchProducts() {
    const query = document.getElementById('searchInput').value.trim(); // Kullanýcýnýn girdiði sorgu

    if (query === "") {
        updateTable(allProducts); // Arama kutusu boþsa, tüm ürünleri göster
        return;
    }

    const apiUrl = `http://127.0.0.1:5000/search?q=${encodeURIComponent(query)}`; // Sorguyu güvenli hale getir

    try {
        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        // HTTP yanýtýný kontrol et
        if (!response.ok) {
            throw new Error(`Network response was not ok: ${response.statusText}`);
        }

        const results = await response.json(); // JSON formatýndaki sonuçlarý al
        updateTable(results); // Tabloyu yeni sonuçlarla güncelle
    } catch (error) {
        console.error('Fetch error:', error.message);
        alert('Ürünleri ararken bir hata oluþtu. Lütfen tekrar deneyin.');
    }
}

function updateTable(products) {
    const tableBody = document.getElementById('productTableBody');
    tableBody.innerHTML = ''; // Eski tabloyu temizle

    if (products.length === 0) {
        const row = document.createElement('tr');
        row.innerHTML = `<td colspan="6" class="text-center">Hiç ürün bulunamadý.</td>`;
        tableBody.appendChild(row);
        return;
    }

    products.forEach(product => {
        const row = document.createElement('tr');

        // Kategori adý doðrudan product.CategoryName'den alýnýyor
        const categoryName = product.CategoryName || 'Bilinmiyor';

        row.innerHTML = `
            <td class="product-name">${product.ProductName}</td>
            <td>${product.Description}</td>
            <td><img src="/Urunler/${product.ImageUrl}" width="25px" height="25px" /></td>
            <td>${product.Price}</td>
            <td>${categoryName}</td> <!-- CategoryName burada kullanýlýyor -->
            <td>
                <a href="/Admin/Edit/${product.ProductId}">Edit</a> |
                <a href="/Admin/Details/${product.ProductId}">Details</a> |
                <a href="/Admin/Delete/${product.ProductId}">Delete</a>
            </td>
        `;
        tableBody.appendChild(row); // Yeni satýrlarý tabloya ekle
    });
}

// Frontend filtreleme (arama kutusuna girilen metinle yerel tabloyu filtreler)
function filterProducts() {
    const query = document.getElementById('searchInput').value.toLowerCase(); // Kullanýcýnýn girdiði sorgu
    const rows = document.querySelectorAll('#productTableBody tr');

    rows.forEach(row => {
        const productName = row.querySelector('.product-name').textContent.toLowerCase();
        if (productName.includes(query)) {
            row.style.display = ''; // Satýrý görünür yap
        } else {
            row.style.display = 'none'; // Satýrý gizle
        }
    });
}

// Arama çubuðunu sýfýrla
function resetFilter() {
    document.getElementById('searchInput').value = ''; // Arama çubuðunu temizle
    updateTable(allProducts); // Tüm ürünleri yeniden getir
}

// Sayfa yüklendiðinde tüm ürünleri getir
document.addEventListener('DOMContentLoaded', function () {
    loadAllProducts(); // Sayfa yüklendiðinde tüm ürünleri getir
    document.getElementById('searchInput').addEventListener('input', filterProducts); // Arama kutusunu dinle
});
