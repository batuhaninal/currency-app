const selectedTable = document.querySelector("#selectedAssets tbody");

    document.getElementById("clearAllBtn").addEventListener("click", function () {
        const selectedRows = document.querySelectorAll("#selectedAssets tbody tr");

        selectedRows.forEach(row => {
            const id = row.dataset.id;
            const count = parseInt(row.dataset.count);

            // Kaynak satırı bul ve mevcut adedi geri güncelle
            const sourceRow = document.querySelector(`#assetTable tr[data-id="${id}"]`);
            if (sourceRow) {
                let available = parseInt(sourceRow.dataset.available);
                available += count;
                sourceRow.dataset.available = available;
                sourceRow.querySelector("td:nth-child(3)").textContent = available;
            }

            row.remove();
        });

        updateTotals();
    });

    document.querySelectorAll('.add-all-calculator-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const row = this.closest('tr');
            const input = row.querySelector('.count-input');
            const available = parseInt(row.dataset.available);

            if (available < 1) {
                alert("Bu varlık için eklenebilecek adet kalmadı.");
                return;
            }

            input.value = available;
            row.querySelector('.add-calculator-btn').click(); // mevcut Ekle butonunu tetikle
        });
    });

    document.querySelectorAll('.add-calculator-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const row = this.closest('tr');
            const id = row.dataset.id;
            const title = row.dataset.title;
            const purchase = parseFloat(row.dataset.purchase);
            const sale = parseFloat(row.dataset.sale);
            const input = row.querySelector('.count-input');
            const count = parseInt(input.value);
            let available = parseInt(row.dataset.available);

            if (!count || count < 1 || count > available) {
                alert("Geçerli bir adet giriniz.");
                return;
            }

            // Mevcut adedi azalt
            available -= count;
            row.dataset.available = available;
            row.querySelector("td:nth-child(3)").textContent = available;

            let existingRow = selectedTable.querySelector(`tr[data-id="${id}"]`);

            if (existingRow) {
                const currentCount = parseInt(existingRow.dataset.count);
                const newCount = currentCount + count;
                existingRow.dataset.count = newCount;
                updateRow(existingRow, newCount, purchase, sale);
            } else {
                const newRow = document.createElement("tr");
                newRow.dataset.id = id;
                newRow.dataset.count = count;
                newRow.dataset.maxcount = available;
                newRow.dataset.purchase = purchase;
                newRow.dataset.sale = sale;

                newRow.innerHTML = `
    <td>${title}</td>
    <td class="count-cell">
        <button type="button" class="btn btn-sm btn-secondary decrement-btn">−</button>
        <input type="number" class="form-control form-control-sm count-edit-input d-inline-block mx-2 text-center"
       style="width: 80px;" min="1" max="${available}" value="${count}" />
        <button type="button" class="btn btn-sm btn-secondary increment-btn">+</button>
    </td>
    <td class="purchase-cell">${(count * purchase).toFixed(2)}</td>
    <td class="sale-cell">${(count * sale).toFixed(2)}</td>
    <td><button type="button" class="btn btn-sm btn-danger remove-row-btn">🗑</button></td>
`;

                selectedTable.appendChild(newRow);

                attachButtons(newRow, row, purchase, sale);
            }

            input.value = "";
            updateTotals();
        });
    });


    function updateRow(row, count, purchase, sale) {
        row.querySelector(".count-cell span").textContent = count;
        row.querySelector(".purchase-cell").textContent = (count * purchase).toFixed(2);
        row.querySelector(".sale-cell").textContent = (count * sale).toFixed(2);
    }

    function attachButtons(row) {
        const decrementBtn = row.querySelector(".decrement-btn");
        const incrementBtn = row.querySelector(".increment-btn");
        const countInput = row.querySelector(".count-edit-input");
        const purchaseCell = row.querySelector(".purchase-cell");
        const saleCell = row.querySelector(".sale-cell");
        const id = row.dataset.id;
        const purchase = parseFloat(row.dataset.purchase);
        const sale = parseFloat(row.dataset.sale);

        decrementBtn.addEventListener("click", function () {
            let count = parseInt(countInput.value);
            if (count > 1) {
                countInput.value = count - 1;
                countInput.dispatchEvent(new Event("change"));
            }
        });

        incrementBtn.addEventListener("click", function () {
            let count = parseInt(countInput.value);
            const max = parseInt(row.dataset.maxcount);
            if (count < max) {
                countInput.value = count + 1;
                countInput.dispatchEvent(new Event("change"));
            }
        });

        countInput.addEventListener("change", function () {
            let newCount = parseInt(this.value);

            const id = row.dataset.id;
            const sourceRow = document.querySelector(`#assetTable tr[data-id="${id}"]`);
            let currentAvailable = parseInt(sourceRow.dataset.available);
            let previousCount = parseInt(row.dataset.count);

            // Kullanıcının toplam sahip olabileceği miktar = eldeki + daha önce seçtiği miktar
            const max = currentAvailable + previousCount;

            if (!newCount || newCount < 1 || newCount > max) {
                alert("Geçerli bir adet giriniz.");
                this.value = previousCount;
                return;
            }

            const diff = newCount - previousCount;
            row.dataset.count = newCount;

            // Elde kalan güncellemesi
            sourceRow.dataset.available = currentAvailable - diff;
            sourceRow.querySelector("td:nth-child(3)").textContent = currentAvailable - diff;

            purchaseCell.textContent = (newCount * purchase).toFixed(2);
            saleCell.textContent = (newCount * sale).toFixed(2);

            updateTotals();
        });


        const removeBtn = row.querySelector(".remove-row-btn");
        removeBtn.addEventListener("click", function () {
            const id = row.dataset.id;
            const count = parseInt(row.dataset.count);

            const sourceRow = document.querySelector(`#assetTable tr[data-id="${id}"]`);
            if (sourceRow) {
                let available = parseInt(sourceRow.dataset.available);
                available += count;
                sourceRow.dataset.available = available;
                sourceRow.querySelector("td:nth-child(3)").textContent = available;
            }

            row.remove();
            updateTotals();
        });
    }


    function updateTotals() {
        let totalPurchase = 0;
        let totalSale = 0;

        document.querySelectorAll("#selectedAssets tbody tr").forEach(row => {
            const purchase = parseFloat(row.querySelector(".purchase-cell").textContent);
            const sale = parseFloat(row.querySelector(".sale-cell").textContent);
            totalPurchase += purchase;
            totalSale += sale;
        });

        document.getElementById("totalPurchase").textContent = totalPurchase.toFixed(2);
        document.getElementById("totalSale").textContent = totalSale.toFixed(2);
    }