const list = document.querySelector('.list');
const showMore = document.querySelector('.but');

// Вычисляем высоту списка и устанавливаем ее в качестве max-height
list.style.maxHeight = `${list.scrollHeight}px`;

// Обработчик события на кнопке "Показать еще"
showMore.addEventListener('click', () => {
    // Если список скрыт, отображаем его, устанавливая высоту равной его полной высоте,
    // и меняем текст кнопки на "Скрыть"
    if (list.style.maxHeight === '0px') {
        list.style.maxHeight = `${list.scrollHeight}px`;
        showMore.innerText = 'Скрыть';
    } else {
        // Иначе скрываем список и меняем текст кнопки на "Показать еще"
        list.style.maxHeight = '0px';
        showMore.innerText = 'Показать Еще';
    }
});
