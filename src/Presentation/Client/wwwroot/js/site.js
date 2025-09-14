// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    const navLinks = document.querySelectorAll(".sidebar-menu .nav-link");
    const currentPath = window.location.pathname;
    

    // Sayfa yüklenince URL'ye göre aktif menü ayarla
    navLinks.forEach(link => {
        // Önce tüm aktifleri temizle
        link.classList.remove("active");
        
        if (link.getAttribute("href") === currentPath) {
            link.classList.add("active");

            // Üst parent menüleri aç
            let parent = link.closest(".nav-treeview");
            while (parent) {
                const parentLink = parent.previousElementSibling;
                if (parentLink) {
                    parentLink.classList.add("active");
                    parentLink.closest(".nav-item").classList.add("menu-open");
                }
                parent = parent.parentElement.closest(".nav-treeview");
            }
        }
    });

    // Tıklama olaylarını dinle -> For SPA
    // navLinks.forEach(link => {
    //     link.addEventListener("click", function (e) {
    //         const parentItem = this.closest(".nav-item");

    //         // Alt menü varsa sadece aç/kapa yap
    //         if (parentItem.querySelector(".nav-treeview")) {
    //             e.preventDefault();
    //             parentItem.classList.toggle("menu-open");
    //             this.classList.toggle("active");
    //             return;
    //         }

    //         // Önce tüm aktifleri temizle
    //         navLinks.forEach(l => l.classList.remove("active"));

    //         // Yeni aktif linki ekle
    //         this.classList.add("active");

    //         // Üst parentleri de aktif yap
    //         let parent = this.closest(".nav-treeview");
    //         while (parent) {
    //             const parentLink = parent.previousElementSibling;
    //             if (parentLink) {
    //                 parentLink.classList.add("active");
    //                 parentLink.closest(".nav-item").classList.add("menu-open");
    //             }
    //             parent = parent.parentElement.closest(".nav-treeview");
    //         }
    //     });
    // });
});



function deletePopup(uri, title = 'Silme Islemi', text = 'Silinmiş veri geri alınamaz') {
  const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
      confirmButton: "btn btn-danger",
      cancelButton: "btn btn-secondary"
    },
    buttonsStyling: true
  });

  swalWithBootstrapButtons.fire({
    title: title,
    text: text,
    icon: "warning",
    showCancelButton: true,
    confirmButtonText: "Sil!",
    cancelButtonText: "İptal!",
    reverseButtons: true
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        url: uri,
        method: "POST",
        success: function (response) {
          if (response.success) {
            swalWithBootstrapButtons.fire({
              title: "Silindi!",
              text: "Silme işlemi başarıyla gerçekleşti.",
              icon: "success"
            }).then(() => {
              location.reload();
            });
          } else {
            swalWithBootstrapButtons.fire({
              title: "Hata!",
              text: response.message || "Silme işlemi başarısız oldu.",
              icon: "error"
            });
          }
        },
        error: function () {
          swalWithBootstrapButtons.fire({
            title: "Beklenmeyen Hata!",
            text: "İstek oluşturulamadı.",
            icon: "error"
          });
        }
      });
    }
  });
}

function statusPopup(uri, title = 'Durum Degistirme Islemi', text = 'Pasif veriler kullanicilar tarafindan gorulemez') {
  const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
      confirmButton: "btn btn-success",
      cancelButton: "btn btn-secondary"
    },
    buttonsStyling: true
  });

  swalWithBootstrapButtons.fire({
    title: title,
    text: text,
    icon: "info",
    showCancelButton: true,
    confirmButtonText: "Guncelle!",
    cancelButtonText: "İptal!",
    reverseButtons: true
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        url: uri,
        method: "POST",
        success: function (response) {
          if (response.success) {
            swalWithBootstrapButtons.fire({
              title: "Guncellendi!",
              text: "Veri durumu başarıyla guncellendi.",
              icon: "success"
            }).then(() => {
              location.reload();
            });
          } else {
            swalWithBootstrapButtons.fire({
              title: "Hata!",
              text: response.message || "Guncelleme işlemi başarısız oldu.",
              icon: "error"
            });
          }
        },
        error: function () {
          swalWithBootstrapButtons.fire({
            title: "Beklenmeyen Hata!",
            text: "İstek oluşturulamadı.",
            icon: "error"
          });
        }
      });
    }
  });
}

function openPopup(targetObj, popupSelector, uri) {
  $.ajax({
    url: uri,
    method: "GET",
    success: function (html) {
      targetObj.html(html);
      $(popupSelector).modal("show");
    },
    error: function () {
      Swal.fire({
        icon: "error",
        title: "Oops...",
        text: "Something went wrong!"
      });
    }
  });
}
