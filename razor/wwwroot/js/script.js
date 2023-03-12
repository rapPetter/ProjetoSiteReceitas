
const form = document.getElementById("formulario-cadastro");


const tituloReceita = document.body.querySelector("#titulo");
const labelTitulo = document.getElementById("labelTitulo");
const tempoPreparoReceita = document.body.querySelector("#tempoPreparo");
const porcoesReceita = document.body.querySelector("#porcoes");
const ingredientesReceita = document.body.querySelector("#ingredientes");
const descricaoReceita = document.body.querySelector("#descricao");


// validar os inputs do formulario
document.getElementById("titulo").addEventListener('keyup', () => {
    if (document.getElementById("titulo").value.length <= 4) {
        labelTitulo.setAttribute('style', 'color: red')
        labelTitulo.innerHTML = 'Título da Receita  *Insira no mínimo 4 caracteres'
        document.getElementById("titulo").setAttribute('style', 'border-color: red')
    }
    else {
        labelTitulo.setAttribute('style', 'color: green')  
        document.getElementById("labelTempoPreparo").setAttribute('style',  'font-size: 10px')
        labelTitulo.innerHTML = 'Título da Receita'
        tituloReceita.setAttribute('style', 'border-color: green')
    }
})
document.getElementById("tempoPreparo").addEventListener('click', () => {
    if (document.getElementById("tempoPreparo").value <= 0) {
        document.getElementById("labelTempoPreparo").setAttribute('style', 'color: red')
        document.getElementById("labelTempoPreparo").innerHTML = '*Insira o tempo para preparar a receita'
        document.getElementById("tempoPreparo").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelTempoPreparo").setAttribute('style', 'color: green')
        document.getElementById("labelTempoPreparo").setAttribute('style', 'font-size: 10px')
        document.getElementById("labelTempoPreparo").innerHTML = 'Tempo de preparo:'
        document.getElementById("tempoPreparo").setAttribute('style', 'border-color: green')
    }
})
document.getElementById("tempoPreparo").addEventListener('keyup', () => {
    if (document.getElementById("tempoPreparo").value <= 0) {
        document.getElementById("labelTempoPreparo").setAttribute('style', 'color: red')
        document.getElementById("labelTempoPreparo").innerHTML = '*Insira o tempo para preparar a receita'
        document.getElementById("tempoPreparo").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelTempoPreparo").setAttribute('style', 'color: green')
        document.getElementById("labelTempoPreparo").innerHTML = 'Tempo de preparo:'
        document.getElementById("tempoPreparo").setAttribute('style', 'border-color: green')
    }
})
document.getElementById("porcoes").addEventListener('click', () => {
    if (document.getElementById("porcoes").value <= 0) {
        document.getElementById("labelporcoes").setAttribute('style', 'color: red')
        document.getElementById("labelporcoes").innerHTML = '*Insira quantas porções a receita rende'
        document.getElementById("porcoes").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelporcoes").setAttribute('style', 'color: green')
        document.getElementById("labelporcoes").innerHTML = 'Porções:'
        document.getElementById("porcoes").setAttribute('style', 'border-color: green')
    }
})
document.getElementById("porcoes").addEventListener('keyup', () => {
    if (document.getElementById("porcoes").value <= 0) {
        document.getElementById("labelporcoes").setAttribute('style', 'color: red')
        document.getElementById("labelporcoes").innerHTML = '*Insira quantas porções a receita rende'
        document.getElementById("porcoes").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelporcoes").setAttribute('style', 'color: green')
        document.getElementById("labelporcoes").innerHTML = 'Porções:'
        document.getElementById("porcoes").setAttribute('style', 'border-color: green')
    }
})
document.getElementById("ingredientes").addEventListener('keyup', () => {
    if (document.getElementById("ingredientes").value.length <= 5) {
        document.getElementById("labelIngredientes").setAttribute('style', 'color: red')
        document.getElementById("labelIngredientes").innerHTML = '*Insira os ingredientes da receita :'
        document.getElementById("ingredientes").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelIngredientes").setAttribute('style', 'color: green')
        document.getElementById("labelIngredientes").innerHTML = 'Ingredientes da Receita:'
        document.getElementById("ingredientes").setAttribute('style', 'border-color: green')    
    }
})
document.getElementById("descricao").addEventListener('keyup', () => {
    if (document.getElementById("descricao").value.length <= 32) {
        document.getElementById("labelDescricao").setAttribute('style', 'color: red')
        document.getElementById("labelDescricao").innerHTML = '*Insira o modo de preparo. :'
        document.getElementById("descricao").setAttribute('style', 'border-color: red')
    }
    else {
        document.getElementById("labelDescricao").setAttribute('style', 'color: green')
        document.getElementById("labelDescricao").innerHTML = 'Modo de preparo:'
        document.getElementById("descricao").setAttribute('style', 'border-color: green')
    }
})




document.getElementById("btnLimpar").addEventListener('click',() =>{
    form.reset()
  })


form.addEventListener('submit', (event) => {

        event.preventDefault();   

        const newValue = ingredientesReceita.value.replace(/\r?\n/g, ' ');
        const inputs = {
            "email": userName,
            "nomeReceita": tituloReceita.value,
            "tempoPreparo": tempoPreparoReceita.value,
            "porcoes": porcoesReceita.value,
            "ingredientes": newValue,
            "descricao": descricaoReceita.value,
        }
        fetch("http://localhost:5203/api/PostarReceita",
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(inputs),
            }).then(() => {
                form.reset()
                alert("Receita salva.")
                getReceita()
            })   
})
