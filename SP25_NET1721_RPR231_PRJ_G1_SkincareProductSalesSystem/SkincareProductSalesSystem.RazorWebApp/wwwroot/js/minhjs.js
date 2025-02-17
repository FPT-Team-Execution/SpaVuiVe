let questions = document.querySelectorAll('.about');

// Duyệt qua từng câu hỏi
questions.forEach((question, index) => {
    // Lấy tất cả các đáp án của câu hỏi hiện tại
    let answers = question.querySelectorAll('.choose-answer');

    // Duyệt qua từng đáp án và thêm sự kiện click
    answers.forEach(answer => {
        answer.addEventListener('click', function () {
            // Xóa lớp 'choosen' khỏi tất cả các đáp án
            answers.forEach(a => a.classList.remove('choosen'));

            // Thêm lớp 'choosen' vào đáp án đã chọn
            this.classList.add('choosen');
        });
    });
});