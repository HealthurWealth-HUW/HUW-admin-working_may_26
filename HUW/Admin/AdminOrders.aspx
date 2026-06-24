<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminOrders.aspx.cs" Inherits="Admin_AdminOrders" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

<meta charset="utf-8" />

<meta name="viewport" content="width=device-width, initial-scale=1.0" />

<title>Orders Dashboard</title>

<style>

*{
    margin:0;
    padding:0;
    box-sizing:border-box;
}

body{
    font-family:Segoe UI,Arial,sans-serif;
    background:#f5f5f5;
    color:#333;
}

.container{
    width:95%;
    max-width:1500px;
    margin:auto;
    padding:20px;
}

.page-header{
    margin-bottom:25px;
}

.page-header h1{
    font-size:30px;
    font-weight:600;
}

.filter-section{
    display:flex;
    gap:10px;
    align-items:center;
    flex-wrap:wrap;
    margin-bottom:25px;
}

.filter-btn{
    padding:10px 18px;
    background:#fff;
    border:1px solid #ddd;
    cursor:pointer;
    border-radius:5px;
}

.filter-btn.active{
    background:#1D9E75;
    color:#fff;
    border-color:#1D9E75;
}

.filter-btn:hover{
    border-color:#1D9E75;
}

.date-picker-wrapper{
    display:flex;
    gap:8px;
    align-items:center;
}

.date-picker-wrapper input{
    padding:8px;
}

.kpi-grid{
    display:grid;
    grid-template-columns:repeat(auto-fit,minmax(280px,1fr));
    gap:20px;
    margin-bottom:30px;
}

.kpi-card{
    background:#fff;
    padding:22px;
    border-radius:8px;
    border:1px solid #e4e4e4;
}

.kpi-label{
    font-size:13px;
    color:#888;
}

.kpi-value{
    font-size:34px;
    font-weight:bold;
    margin-top:12px;
}

.kpi-trend{
    margin-top:10px;
    font-size:14px;
    font-weight:600;
}

.trend-up{
    color:#0c9c2b;
}

.trend-down{
    color:#d92a2a;
}

.chart-card{
    background:#fff;
    border-radius:8px;
    border:1px solid #e4e4e4;
    padding:20px;
    margin-bottom:30px;
}

.chart-title{
    font-size:18px;
    margin-bottom:20px;
    font-weight:600;
}

.chart-container{
    height:360px;
}

.chart-header{
    display:flex;
    justify-content:space-between;
    align-items:center;
    margin-bottom:20px;
}

.table-card{
    background:#fff;
    border-radius:8px;
    border:1px solid #e4e4e4;
    padding:20px;
}

.table-header{
    display:flex;
    justify-content:space-between;
    margin-bottom:15px;
}

.table-title{
    font-size:18px;
    font-weight:600;
}

.export-btn{
    background:#1D9E75;
    color:white;
    border:none;
    padding:10px 18px;
    border-radius:5px;
    cursor:pointer;
}

.export-btn:hover{
    background:#177e5d;
}

table{
    width:100%;
    border-collapse:collapse;
}

th{
    background:#f8f8f8;
    text-align:left;
    padding:12px;
    border-bottom:2px solid #eee;
}

td{
    padding:12px;
    border-bottom:1px solid #eee;
}

.rank-badge{
    display:inline-flex;
    justify-content:center;
    align-items:center;
    width:28px;
    height:28px;
    border-radius:50%;
    background:#1D9E75;
    color:#fff;
}

.stock-badge{
    padding:4px 12px;
    border-radius:4px;
    font-size:12px;
}

.stock-badge.low{
    background:#ffe9a8;
}

.stock-badge.in{
    background:#d8f6df;
}

.pagination{
    display:flex;
    justify-content:center;
    gap:8px;
    margin-top:25px;
}

.pagination button{
    padding:8px 12px;
    cursor:pointer;
}

.pagination .active{
    background:#1D9E75;
    color:white;
}

.loading{
    text-align:center;
    padding:40px;
}

.error-box{
    background:#ffe6e6;
    color:#c62828;
    border:1px solid #f3b3b3;
    padding:10px;
    margin-bottom:20px;
}

</style>

</head>

<body>

<form id="form1" runat="server">

<div class="container">

<div class="page-header">
<h1>Orders Dashboard</h1>
</div>

<div class="filter-section">

<button type="button"
class="filter-btn"
onclick="setDateRange('today')">
Today
</button>

<button type="button"
class="filter-btn active"
onclick="setDateRange('last7')">
Last 7 Days
</button>

<button type="button"
class="filter-btn"
onclick="setDateRange('last30')">
Last 30 Days
</button>

<button type="button"
class="filter-btn"
onclick="setDateRange('thismonth')">
This Month
</button>

<button type="button"
class="filter-btn"
onclick="setDateRange('custom')">
Custom Range
</button>

<div id="customDatePicker"
class="date-picker-wrapper"
style="display:none;">

<input type="date" id="startDate"/>

<span>to</span>

<input type="date" id="endDate"/>

<button
type="button"
class="filter-btn"
onclick="applyCustomRange()">
Apply
</button>

</div>

</div>

<div id="errorMessage"></div>

<div id="kpiGrid" class="kpi-grid">

<div class="loading">
Loading dashboard...
</div>

</div>

<div class="chart-card">

    <div class="chart-header">

        <div class="chart-title" id="chartTitle">
            Top Selling Products (Units Sold)
        </div>

        <select id="chartFilter" onchange="changeChartType()">

            <option value="products" selected>
                Products
            </option>

            <option value="categories">
                Categories
            </option>

        </select>

    </div>

    <div class="chart-container">
        <canvas id="topProductsChart"></canvas>
    </div>

</div>
    <div class="table-card">

    <div class="table-header">
<div class="table-title">
Top Sold Products
</div>

<button
type="button"
class="export-btn"
onclick="exportToCSV()">
Export
</button>

</div>

<table id="productsTable">

<thead>

<tr>

<th>#</th>

<th>Product ID</th>

<th>Product Name</th>

<th>Units Sold</th>

<th>Revenue</th>

<th>Orders</th>

<th>Growth %</th>

<th>Stock Left</th>

<th>Status</th>

</tr>

</thead>

<tbody id="tableBody">

<tr>

<td colspan="9" class="loading">

Loading...

</td>

</tr>

</tbody>

</table>

<div id="pagination" class="pagination"></div>

</div>

</div>

</form>

<script src="https://cdnjs.cloudflare.com/ajax/libs/Chart.js/4.4.1/chart.umd.min.js"></script>

<script>

    var apiBaseUrl = "/api/Master";

    var currentPeriod = "last7";

    var currentPage = 1;

    var chartInstance = null;

    var tableCache = [];
    //====================================================
    // Error Functions
    //====================================================

    function showError(message) {

        document.getElementById("errorMessage").innerHTML =
            '<div class="error-box">' + message + '</div>';
    }

    function clearError() {
        document.getElementById("errorMessage").innerHTML = "";
    }

    //====================================================
    // Date Functions
    //====================================================

    function formatDate(date) {

        var year = date.getFullYear();
        var month = ("0" + (date.getMonth() + 1)).slice(-2);
        var day = ("0" + date.getDate()).slice(-2);

        return year + "-" + month + "-" + day;
    }

    function getDateRange(customStart, customEnd) {

        var today = new Date();

        if (currentPeriod === "custom") {

            return {
                start: customStart,
                end: customEnd
            };
        }
        if (currentPeriod === "today") {

            return {

                start: formatDate(today),

                end: formatDate(today)

            };

        }
        if (currentPeriod === "last7") {

            var start = new Date(today);
            start.setDate(today.getDate() - 7);

            return {
                start: formatDate(start),
                end: formatDate(today)
            };
        }

        if (currentPeriod === "last30") {

            var start = new Date(today);
            start.setDate(today.getDate() - 30);

            return {
                start: formatDate(start),
                end: formatDate(today)
            };
        }

        if (currentPeriod === "thismonth") {

            var start = new Date(today.getFullYear(), today.getMonth(), 1);

            return {
                start: formatDate(start),
                end: formatDate(today)
            };
        }

        return {
            start: formatDate(today),
            end: formatDate(today)
        };
    }

    //====================================================
    // Filter Buttons
    //====================================================

    function setDateRange(period) {

        currentPeriod = period;

        currentPage = 1;

        var buttons = document.querySelectorAll(".filter-btn");

        for (var i = 0; i < buttons.length; i++) {

            buttons[i].classList.remove("active");

        }

        switch (period) {

            case "today":
                buttons[0].classList.add("active");
                break;

            case "last7":
                buttons[1].classList.add("active");
                break;

            case "last30":
                buttons[2].classList.add("active");
                break;

            case "thismonth":
                buttons[3].classList.add("active");
                break;

            case "custom":
                buttons[4].classList.add("active");
                break;

        }

        document.getElementById("customDatePicker").style.display =
            period === "custom" ? "flex" : "none";

        if (period !== "custom") {

            loadDashboard();

        }

    }
    function applyCustomRange() {

        var start = document.getElementById("startDate").value;
        var end = document.getElementById("endDate").value;

        if (start === "" || end === "") {

            showError("Please select both dates.");

            return;
        }

        currentPage = 1;

        loadDashboard(start, end);
    }

    //====================================================
    // Main Dashboard Loader
    //====================================================

    function loadDashboard(customStart, customEnd) {

        clearError();

        var dates = getDateRange(customStart, customEnd);

        loadKPI(dates);

        if (document.getElementById("chartFilter").value == "products") {

            loadChart(dates);

        }
        else {

            loadCategoryChart(dates);

        }

        loadTable(dates);
    }
    //====================================================
    // KPI
    //====================================================

    function loadKPI(dates) {

        var url =
            apiBaseUrl +
            "/GetOrdersKPI?startDate=" +
            dates.start +
            "&endDate=" +
            dates.end;

        fetch(url)

            .then(function (response) {

                if (!response.ok)
                    throw new Error("Unable to load KPI");

                return response.json();

            })

            .then(function (data) {

                if (data.Status !== "Success") {

                    throw new Error(data.Message);

                }

                renderKPICards(data.Result);

            })

            .catch(function (error) {

                console.log(error);

                showError(error.message);

            });

    }

    //====================================================
    // KPI Renderer
    //====================================================

    function renderKPICards(data) {

        var html = "";

        html +=
            '<div class="kpi-card">' +
            '<div class="kpi-label">Total Orders</div>' +
            '<div class="kpi-value">' +
            data.totalOrders +
            '</div>' +
            '<div class="kpi-trend ' +
            (data.totalOrdersTrend >= 0 ? "trend-up" : "trend-down") +
            '">' +
            (data.totalOrdersTrend >= 0 ? "▲ " : "▼ ") +
            Math.abs(data.totalOrdersTrend).toFixed(2) +
            '%</div>' +
            "</div>";

        html +=
            '<div class="kpi-card">' +
            '<div class="kpi-label">Completed Orders</div>' +
            '<div class="kpi-value">' +
            data.completedOrders +
            '</div>' +
            '<div class="kpi-trend ' +
            (data.completedOrdersTrend >= 0 ? "trend-up" : "trend-down") +
            '">' +
            (data.completedOrdersTrend >= 0 ? "▲ " : "▼ ") +
            Math.abs(data.completedOrdersTrend).toFixed(2) +
            '%</div>' +
            "</div>";

        html +=
            '<div class="kpi-card">' +
            '<div class="kpi-label">Products Sold</div>' +
            '<div class="kpi-value">' +
            data.productsSold +
            '</div>' +
            '<div class="kpi-trend ' +
            (data.productsSoldTrend >= 0 ? "trend-up" : "trend-down") +
            '">' +
            (data.productsSoldTrend >= 0 ? "▲ " : "▼ ") +
            Math.abs(data.productsSoldTrend).toFixed(2) +
            '%</div>' +
            "</div>";

        document.getElementById("kpiGrid").innerHTML = html;
    }

    //====================================================
    // Chart Loader
    //====================================================

    function loadChart(dates) {

        var url =
            apiBaseUrl +
            "/GetTopProductsByOrders?startDate=" +
            dates.start +
            "&endDate=" +
            dates.end;

        fetch(url)

            .then(function (response) {

                if (!response.ok)
                    throw new Error("Unable to load chart");

                return response.json();

            })

            .then(function (data) {

                if (data.Status !== "Success")
                    throw new Error(data.Message);

                renderTopProductsChart(data.Result);

            })

            .catch(function (error) {

                console.log(error);

                showError(error.message);

            });

    }
    //====================================================
    // Change Chart Type
    //====================================================

    function changeChartType() {

        var type = document.getElementById("chartFilter").value;

        var dates = getDateRange(
            document.getElementById("startDate").value,
            document.getElementById("endDate").value
        );

        if (type == "products") {

            document.getElementById("chartTitle").innerHTML =
                "Top Selling Products (Units Sold)";

            loadChart(dates);

        }
        else {

            document.getElementById("chartTitle").innerHTML =
                "Top Selling Categories (Units Sold)";

            loadCategoryChart(dates);

        }
    }

    function loadCategoryChart(dates) {

        var url =
            apiBaseUrl +
            "/GetTopCategoriesByOrders?startDate=" +
            dates.start +
            "&endDate=" +
            dates.end;

        fetch(url)

            .then(function (response) {

                if (!response.ok)
                    throw new Error("Unable to load category chart");

                return response.json();

            })

            .then(function (data) {

                if (data.Status !== "Success")
                    throw new Error(data.Message);

                renderCategoryChart(data.Result);

            })

            .catch(function (error) {

                showError(error.message);

            });

    }
    //====================================================
    // Render Category Chart
    //====================================================

    function renderCategoryChart(categories) {

        if (!categories || categories.length == 0)
            return;

        var labels = [];
        var values = [];

        for (var i = 0; i < categories.length; i++) {

            labels.push(categories[i].categoryName);
            values.push(categories[i].unitsSold);

        }

        var ctx = document.getElementById("topProductsChart").getContext("2d");

        if (chartInstance != null)
            chartInstance.destroy();

        chartInstance = new Chart(ctx, {

            type: "bar",

            data: {

                labels: labels,

                datasets: [

                    {

                        label: "Units Sold",

                        data: values,

                        backgroundColor: "#1D9E75",

                        borderRadius: 5

                    }

                ]

            },

            options: {

                responsive: true,

                maintainAspectRatio: false,

                plugins: {

                    legend: {

                        display: false

                    }

                },

                scales: {

                    y: {

                        beginAtZero: true

                    }

                }

            }

        });

    }
    //====================================================
    // Render Chart
    //====================================================

    function renderTopProductsChart(products) {

        if (!products || products.length === 0)
            return;

        var labels = [];
        var values = [];

        for (var i = 0; i < products.length; i++) {

            labels.push(products[i].productName);
            values.push(products[i].unitsSold);

        }

        var ctx = document.getElementById("topProductsChart").getContext("2d");

        if (chartInstance != null)
            chartInstance.destroy();

        chartInstance = new Chart(ctx, {

            type: "bar",

            data: {

                labels: labels,

                datasets: [

                    {

                        label: "Units Sold",

                        data: values,

                        backgroundColor: "#1D9E75",

                        borderRadius: 5

                    }

                ]

            },

            options: {

                responsive: true,

                maintainAspectRatio: false,

                plugins: {

                    legend: {

                        display: false

                    }

                },

                scales: {

                    y: {

                        beginAtZero: true

                    }

                }

            }

        });

    }

    //====================================================
    // Load Table
    //====================================================

    function loadTable(dates) {

        var url =
            apiBaseUrl +
            "/GetOrdersTableData?startDate=" +
            dates.start +
            "&endDate=" +
            dates.end +
            "&pageNumber=" +
            currentPage +
            "&pageSize=10";

        fetch(url)

            .then(function (response) {

                if (!response.ok)
                    throw new Error("Unable to load table.");

                return response.json();

            })

            .then(function (data) {

                if (data.Status !== "Success")
                    throw new Error(data.Message);

                tableCache = data.Result.data;

                renderTable(data.Result.data);

                renderPagination(data.Result.pagination);

            })

            .catch(function (error) {

                console.log(error);

                showError(error.message);

            });

    }

    //====================================================
    // Render Table
    //====================================================

    function renderTable(rows) {

        var tbody = document.getElementById("tableBody");

        tbody.innerHTML = "";

        if (!rows || rows.length === 0) {

            tbody.innerHTML =
                "<tr><td colspan='9' class='loading'>No data found.</td></tr>";

            return;

        }

        for (var i = 0; i < rows.length; i++) {

            var r = rows[i];

            var badgeClass =
                r.stockStatus == "Low Stock"
                    ? "low"
                    : "in";

            var growthClass =
                r.growthPercent >= 0
                    ? "trend-up"
                    : "trend-down";

            var arrow =
                r.growthPercent >= 0
                    ? "▲"
                    : "▼";

            var tr = "";

            tr += "<tr>";

            tr += "<td><span class='rank-badge'>" + r.rank + "</span></td>";

            tr += "<td>" + r.productId + "</td>";

            tr += "<td>" + r.productName + "</td>";

            tr += "<td>" + r.unitsSold + "</td>";

            tr += "<td>₹" + Number(r.revenueGenerated).toLocaleString("en-IN") + "</td>";

            tr += "<td>" + r.orders + "</td>";

            tr += "<td class='" + growthClass + "'>" +
                arrow +
                " " +
                Math.abs(r.growthPercent).toFixed(2) +
                "%</td>";

            tr += "<td>" + r.stockLeft + "</td>";

            tr += "<td><span class='stock-badge " +
                badgeClass +
                "'>" +
                r.stockStatus +
                "</span></td>";

            tr += "</tr>";

            tbody.innerHTML += tr;

        }

    }

    //====================================================
    // Pagination
    //====================================================

    function renderPagination(page) {

        var div = document.getElementById("pagination");

        div.innerHTML = "";

        if (page.totalPages <= 1)
            return;

        // Previous

        div.innerHTML +=
            "<button " +
            (page.pageNumber == 1 ? "disabled" : "") +
            " onclick='goToPage(" +
            (page.pageNumber - 1) +
            ")'>Previous</button>";

        for (var i = 1; i <= page.totalPages; i++) {

            div.innerHTML +=
                "<button type='button' class='" +
                (i == page.pageNumber ? "active" : "") +
                "' onclick='goToPage(" +
                i +
                ")'>" +
                i +
                "</button>";

        }

        // Next

        div.innerHTML +=
            "<button type='button' class='" +
            (page.pageNumber == page.totalPages ? "disabled" : "") +
            " onclick='goToPage(" +
            (page.pageNumber + 1) +
            ")'>Next</button>";

    }

    //====================================================
    // Page Change
    //====================================================

    function goToPage(pageNo) {

        currentPage = pageNo;

        var dates = getDateRange(

            document.getElementById("startDate").value,

            document.getElementById("endDate").value

        );

        loadTable(dates);

    }
    //====================================================
    // Export CSV
    //====================================================

    function exportToCSV() {

        if (!tableCache || tableCache.length === 0) {

            alert("No data available to export.");

            return;
        }

        var csv = [];

        csv.push([
            "Rank",
            "Product ID",
            "Product Name",
            "Units Sold",
            "Revenue",
            "Orders",
            "Growth %",
            "Stock Left",
            "Status"
        ].join(","));

        for (var i = 0; i < tableCache.length; i++) {

            var r = tableCache[i];

            csv.push([
                r.rank,
                r.productId,
                '"' + (r.productName || "").replace(/"/g, '""') + '"',
                r.unitsSold,
                r.revenueGenerated,
                r.orders,
                r.growthPercent,
                r.stockLeft,
                r.stockStatus
            ].join(","));

        }

        var csvFile = new Blob([csv.join("\n")], {
            type: "text/csv"
        });

        var downloadLink = document.createElement("a");

        downloadLink.download = "OrdersDashboard.csv";

        downloadLink.href = window.URL.createObjectURL(csvFile);

        downloadLink.style.display = "none";

        document.body.appendChild(downloadLink);

        downloadLink.click();

        document.body.removeChild(downloadLink);

    }

    //====================================================
    // Dashboard Initialization
    //====================================================

    document.addEventListener("DOMContentLoaded", function () {

        loadDashboard();

    });

</script>

</body>

</html>