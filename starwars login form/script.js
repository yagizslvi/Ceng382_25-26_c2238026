//// Bu .js'de ChatGPT'den yardım aldım.


// Kullanıcı giriş bilgilerini saklayacak dizi
let loginAttempts = [];

// Butonu seç
const loginButton = document.querySelector(".login-submit");

// Butona tıklama event listener ekle
loginButton.addEventListener("click", saveLogin);

// Giriş bilgilerini kaydeden fonksiyon
function saveLogin() {
    // Kullanıcıdan alınan bilgileri seçiyor
    let sithID = document.querySelector(".input-field[type='text']").value;
    let password = document.querySelector(".input-field[type='password']").value;

    // Kullanıcı giriş bilgilerini diziye ekle
    loginAttempts.push({ SithID: sithID, Password: password });

    // Konsola tüm girişleri yazdır
    console.log("Tüm giriş denemeleri:", loginAttempts);
}

// Saat kısmında ChatGPT'den yardım aldım.
// Canlı saat fonksiyonu
function updateClock() {
    let now = new Date();
    let hours = now.getHours().toString().padStart(2, "0");
    let minutes = now.getMinutes().toString().padStart(2, "0");
    let seconds = now.getSeconds().toString().padStart(2, "0");

    // Saati ekrana yazdır
    document.getElementById("clock").textContent = `${hours}:${minutes}:${seconds}`;
}

// Saati her saniye güncelleyior
setInterval(updateClock, 1000);
updateClock(); // Sayfa açılır açılmaz çalıştırıyor.


// 'H' tuşuna basınca login formunu gizle/göster.
// 'H' tuşunda Chat GPT'den yardım aldım.
document.addEventListener("keydown", function (event) {
    if (event.key === "h" || event.key === "H") {
        let loginBox = document.querySelector(".login-container");

        // Eğer form görünüyorsa gizle, gizliyse göster.
        if (loginBox.style.display === "none" || loginBox.style.opacity === "0") {
            loginBox.style.display = "block";
            setTimeout(() => { loginBox.style.opacity = "1"; }, 10); // Görünürlüğü yavaşça artır
        } else {
            loginBox.style.opacity = "0";
            setTimeout(() => { loginBox.style.display = "none"; }, 500); // Geçiş efekti ver
        }
    }
});

document.addEventListener("DOMContentLoaded", function() {
    console.log("script.js yüklendi!");

    // Login butonunu seç
    const loginButton = document.querySelector(".login-submit");

    if (loginButton) {
        loginButton.addEventListener("click", function(event) {
            event.preventDefault(); // Sayfa yenilemeyi engelle

            // Kullanıcı bilgilerini al
            let username = document.querySelector(".input-field[type='text']").value;
            let password = document.querySelector(".input-field[type='password']").value;

            // Giriş doğrulaması
            if (username === "admin" && password === "admin") {
                console.log("Başarılı giriş! Table sayfasına yönlendiriliyor...");
                window.location.href = "table.html"; // Kullanıcıyı yönlendir
            } else {
                alert("Hatalı Sith ID veya Authentication Code!");
            }
        });
    } else {
        console.error("Login butonu bulunamadı!");
    }
});

