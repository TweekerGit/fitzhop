const plusik = document.getElementById('plusik');
const listToShow = document.querySelector('.nav_hidden');
let clsElements = document.querySelectorAll(".furniture--type");

plusik.addEventListener('click', event => {
    listToShow.classList.toggle('toshow');
})

clsElements.forEach(element => {
    element.addEventListener('click', event => {
        listToShow.classList.toggle('toshow')
    })
})