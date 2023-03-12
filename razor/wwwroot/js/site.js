

const minhaLista = document.getElementById('cadastroInterno');
const campoBusca = document.getElementById('search');
const btnBusca = document.getElementById('btnPesquisa');
const mostrarBuscaOK = document.getElementById("mostrarCategoira");
const btnLimparBusca = document.getElementById("btnLimparCampos");



function getReceita() {
  document.getElementById("cadastroInterno").innerHTML = "";
  fetch('http://localhost:5148/api/Receitas?status=1')
    .then(response => response.json())
    .then(data => {
      data.forEach(receita => {
        minhaLista.setAttribute('style', 'color: black')
        let li = document.createElement('li');
        let div = document.createElement('div');
        let data = new Date(receita.dataPublicacao);
        let dataFormatada = data.toLocaleDateString();

        div.innerHTML = `<h2>${receita.nomeReceita}</h2>
                      <h5>Ingredientes:</h5>
                       <samp> ${receita.ingredientes}</samp>
                       
                       
                       <h5>Modo de preparo:</h5>
                       <samp> ${receita.descricao}</samp>
                       <p> </p>
                       <div id="conteinerImagensBanco">
                       <p><samp class="colocaMaior">Categoria:</samp> <samp> ${receita.categoria}</samp></p>
                       <div><img src="./images/tempoPreparo.png" alt="tempo para preparar a camida."> <samp class="imageSpam"> ${receita.tempoPreparo} Minutos</samp></div>            
                       <div><img src="./images/prato.png" alt="porções que o prato rende."> <samp class="imageSpam"> ${receita.porcoes} Porções </samp></div>         
                       <div><img src="./images/calorias.png" alt="calorias por receita."><samp class="imageSpam"> ${receita.caloria} Calorias aproximadas </samp></div>                          
                       <div><img src="./images/data.png" alt="Publicado em:"><samp class="imageSpam">  ${dataFormatada}</samp>  </div>         
                       </div>
                       `;
        li.appendChild(div)
        minhaLista.appendChild(li)
      });
    });

}
getReceita()

btnBusca.addEventListener('click', () => {
  document.getElementById("cadastroInterno").innerHTML = "";
  fetch('http://localhost:5148/api/Receitas?status=1')
    .then(response => response.json())
    .then(data => {
      const filteredList = data.filter((x) =>
        x.nomeReceita.toLocaleLowerCase().includes(campoBusca.value.toLocaleLowerCase())
      )
      if (filteredList.length === 0) {
        minhaLista.setAttribute('style', 'color: red')
        minhaLista.innerHTML = ` Nenhuma receita registrada foi encontrado com esse título: <strong><h4>" ${campoBusca.value} "</h4></strong>  <br>
        Clique no botão limpar ou digite outro título de receita.`;
      } else {
        filteredList.forEach(receita => {
          minhaLista.setAttribute('style', 'color: black')
          let li = document.createElement('li');
          let div = document.createElement('div');
          let data = new Date(receita.dataPublicacao);
          let dataFormatada = data.toLocaleDateString();

          div.innerHTML = `<h2>${receita.nomeReceita}</h2>
                          <h5>Ingredientes:</h5>
                           <samp> ${receita.ingredientes}</samp>
                           
                           
                           <h5>Modo de preparo:</h5>
                           <samp> ${receita.descricao}</samp>
                           <p> </p>
                           <div id="conteinerImagensBanco">
                           <p><samp class="colocaMaior">Categoria:</samp> <samp> ${receita.categoria}</samp></p>
                           <div><img src="./images/tempoPreparo.png" alt="tempo para preparar a camida."> <samp class="imageSpam"> ${receita.tempoPreparo} Minutos</samp></div>            
                           <div><img src="./images/prato.png" alt="porções que o prato rende."> <samp class="imageSpam"> ${receita.porcoes} Porções </samp></div>         
                           <div><img src="./images/calorias.png" alt="calorias por receita."><samp class="imageSpam"> ${receita.caloria} Calorias aproximadas </samp></div>                          
                           <div><img src="./images/data.png" alt="Publicado em:"><samp class="imageSpam">  ${dataFormatada}</samp>  </div>         
                           </div>
                           `;
          li.appendChild(div)
          minhaLista.appendChild(li)
        });
      }
    });
})

btnLimparBusca.addEventListener('click', () => {
  campoBusca.value = '';
  getReceita()
})

