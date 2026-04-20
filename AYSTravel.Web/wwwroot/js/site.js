// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
@section Scripts{
    <script>

        function updateCountdowns() {

            document.querySelectorAll(".countdown").forEach(el => {

                let matchDate = new Date(el.dataset.date);
                let now = new Date();
                let diff = matchDate - now;

                if (diff <= 0) {
                    el.innerHTML = "🔴 Match en cours";
                    return;
                }

                let days = Math.floor(diff / (1000 * 60 * 60 * 24));
                let hours = Math.floor((diff / (1000 * 60 * 60)) % 24);
                let minutes = Math.floor((diff / (1000 * 60)) % 60);
                let seconds = Math.floor((diff / 1000) % 60);

                el.innerHTML = `⏳ ${days}j ${hours}h ${minutes}m${ seconds }s`;
            });
}

        setInterval(updateCountdowns, 1000);
        updateCountdowns();


        /* FILTRE VILLE */
        document.getElementById("villeFilter")
        .addEventListener("change", function () {

            let value = this.value;

        document.querySelectorAll(".match-item")
        .forEach(item => {

            item.style.display =
            !value || item.dataset.ville === value
                ? ""
                : "none";
    });
});

    </script>
}