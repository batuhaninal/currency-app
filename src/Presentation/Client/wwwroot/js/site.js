// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
