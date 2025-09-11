const connection = new signalR
      .HubConnectionBuilder()
      .withUrl("https://localhost:7250/hubs/currencies")
      .build();


connection.on("ReceivePrice", function (response) {
    console.log("Event listening for " + response.currencyId);
    console.log(response);
    const card = document.getElementById(`card-${response.currencyId}`);

    if (card) {
        card.querySelector("#currency-card-title-"+response.currencyId).innerText = response.title;
        card.querySelector("#currency-purchase-"+response.currencyId).innerText = response.purchasePrice;
        card.querySelector("#currency-sale-"+response.currencyId).innerText = response.salePrice;

        // opsiyonel: fiyat değişince renk animasyonu
        card.classList.add(response.purchasePriceChanged > 0 ? "price-up" : "price-down");
        setTimeout(() => card.classList.remove("price-up", "price-down"), 1000);
      }
});

// connection.start()
//       .then(() => {
//         console.log("SignalR connected");
//       })
//       .catch(err => console.error(err));

function subscribeToCurrency(currencyId) {
    if (connection.state !== signalR.HubConnectionState.Connected) {
        // bağlantı hazır değilse start et ve sonra abone ol
        connection.start().then(() => {
            return connection.invoke("SubscribeToCurrency", currencyId);
        })
        .then(() => console.log("Subscribed to currency-" + currencyId))
        .catch(err => console.error("Subscribe failed:", err));
    } else {
        connection.invoke("SubscribeToCurrency", currencyId)
            .then(() => console.log("Subscribed to currency-" + currencyId))
            .catch(err => console.error("Subscribe failed:", err));
    }
}
