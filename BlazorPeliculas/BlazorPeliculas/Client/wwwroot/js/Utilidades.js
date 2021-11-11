function pruebaDotNetStatic() {
    DotNet.invokeMethodAsync("BlazorPeliculas.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log("conteo desde js " + resultado);
        });
}

function pruebaDotNetInstancia(dotNetHelper) {
    dotNetHelper.invokeMethodAsync("IncrementCount");
}