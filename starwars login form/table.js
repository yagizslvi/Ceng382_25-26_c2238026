// Bu .js'de ChatGPT'den yardım aldım.


document.addEventListener("DOMContentLoaded", function() {
    console.log("table.js yüklendi!");

    // Form ve tabloyu seç
    const classForm = document.getElementById("class-form");
    const classTable = document.querySelector("#class-table tbody");

    // Eğer form bulunamazsa hata gösterme
    if (!classForm) {
        console.error("Form bulunamadı!");
        return;
    }

    // Form gönderildiğinde tabloya veri ekle
    classForm.addEventListener("submit", function(event) {
        event.preventDefault(); // Sayfanın yenilenmesini engelle

        // Input'lardan değerleri al
        let className = document.getElementById("class-name").value.trim();
        let classSize = document.getElementById("class-size").value.trim();
        let classDesc = document.getElementById("class-desc").value.trim();

        // Eğer boş bırakılmışsa ekleme yapma
        if (!className || !classSize || !classDesc) {
            alert("Lütfen tüm alanları doldurun!");
            return;
        }

        // Yeni bir satır oluştur
        let newRow = document.createElement("tr");
        newRow.innerHTML = `
            <td>${className}</td>
            <td>${classSize}</td>
            <td>${classDesc}</td>
        `;

        // Tabloya ekle
        classTable.appendChild(newRow);

        // Input'ları temizle
        classForm.reset();
    });

    // Tablo satırlarına tıklama efekti
    const tableBody = document.querySelector("tbody");
    if (tableBody) {
        tableBody.addEventListener("click", function(event) {
            if (event.target.tagName === "TD") {
                console.log("Tıklanan satır:", event.target.parentElement.innerText);
            }
        });

        tableBody.addEventListener("mouseover", function(event) {
            if (event.target.tagName === "TD") {
                event.target.parentElement.style.backgroundColor = "#ffeb3b";
            }
        });

        tableBody.addEventListener("mouseout", function(event) {
            if (event.target.tagName === "TD") {
                event.target.parentElement.style.backgroundColor = "";
            }
        });
    }
});
