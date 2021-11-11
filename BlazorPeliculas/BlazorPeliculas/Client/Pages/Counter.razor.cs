using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;

namespace BlazorPeliculas.Client.Pages
{
    public partial class Counter
    {
        [Inject] IJSRuntime JS { get; set; }

        IJSObjectReference modulo;

        private int currentCount = 0;
        static int currentCountStatic = 0;

        [JSInvokable]
        public async Task IncrementCount()
        {
            var arreglo = new double[] { 1, 2, 3, 4, 5 };
            var max = arreglo.Maximum();
            var min = arreglo.Minimum();

            //Cargar los modulos JS cuando sean necesarios
            modulo = await JS.InvokeAsync<IJSObjectReference>("import", "./js/Counter.js");
            await modulo.InvokeVoidAsync("mostrarAlerta", $"El Max es {max} y el Min es {min}");

            currentCount++;
            currentCountStatic++;
            await JS.InvokeVoidAsync("pruebaDotNetStatic");
        }

        protected async Task IncrementCountJS()
        {
            await JS.InvokeVoidAsync("pruebaDotNetInstancia",
                    DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public static Task<int> ObtenerCurrentCount()
        {
            return Task.FromResult(currentCountStatic);
        }
    }
}
