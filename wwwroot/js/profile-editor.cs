document.getElementById('addDependente').addEventListener('click', function () {
    var container = document.getElementById('dependentesContainer');
    var index = container.children.length; // Obtém o próximo índice para a lista

    var newDependenteHtml = `
        <div class="dependente-item mt-3 border p-3 rounded">
            <div class="form-group">
                <label for="Dependentes_${index}__Nome">Nome do Dependente</label>
                <input type="text" id="Dependentes_${index}__Nome" name="Dependentes[${index}].Nome" class="form-control" />
                <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].Nome" data-valmsg-replace="true"></span>
            </div>
            <div class="form-group">
                <label for="Dependentes_${index}__DataNascimento">Data de Nascimento do Dependente</label>
                <input type="date" id="Dependentes_${index}__DataNascimento" name="Dependentes[${index}].DataNascimento" class="form-control" />
                <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].DataNascimento" data-valmsg-replace="true"></span>
            </div>
            <button type="button" class="btn btn-danger remove-dependente mt-2">Remover</button>
        </div>
    `;
    container.insertAdjacentHTML('beforeend', newDependenteHtml);
});

document.getElementById('dependentesContainer').addEventListener('click', function (e) {
    if (e.target && e.target.classList.contains('remove-dependente')) {
        e.target.closest('.dependente-item').remove();
        // Reajustar os índices dos inputs para que o model binding funcione corretamente
        reindexDependentes();
    }
});

function reindexDependentes() {
    var dependenteItems = document.querySelectorAll('#dependentesContainer .dependente-item');
    dependenteItems.forEach(function (item, index) {
        item.querySelectorAll('input').forEach(function (input) {
            var name = input.name;
            if (name) {
                input.name = name.replace(/\[\d+\]/, '[' + index + ']');
                input.id = name.replace(/\[\d+\]/, '_' + index + '__').replace(/\./g, '_');
            }
        });
        item.querySelectorAll('label').forEach(function (label) {
            var htmlFor = label.htmlFor;
            if (htmlFor) {
                label.htmlFor = htmlFor.replace(/_\d+__/, '_' + index + '__');
            }
        });
        item.querySelectorAll('span[data-valmsg-for]').forEach(function (span) {
            var dataValmsgFor = span.getAttribute('data-valmsg-for');
            if (dataValmsgFor) {
                span.setAttribute('data-valmsg-for', dataValmsgFor.replace(/\[\d+\]/, '[' + index + ']'));
            }
        });
    });
}document.getElementById('addDependente').addEventListener('click', function () {
    var container = document.getElementById('dependentesContainer');
    var index = container.children.length; // Obtém o próximo índice para a lista

    var newDependenteHtml = `
        <div class="dependente-item mt-3 border p-3 rounded">
            <div class="form-group">
                <label for="Dependentes_${index}__Nome">Nome do Dependente</label>
                <input type="text" id="Dependentes_${index}__Nome" name="Dependentes[${index}].Nome" class="form-control" />
                <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].Nome" data-valmsg-replace="true"></span>
            </div>
            <div class="form-group">
                <label for="Dependentes_${index}__DataNascimento">Data de Nascimento do Dependente</label>
                <input type="date" id="Dependentes_${index}__DataNascimento" name="Dependentes[${index}].DataNascimento" class="form-control" />
                <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].DataNascimento" data-valmsg-replace="true"></span>
            </div>
            <button type="button" class="btn btn-danger remove-dependente mt-2">Remover</button>
        </div>
    `;
    container.insertAdjacentHTML('beforeend', newDependenteHtml);
});

document.getElementById('dependentesContainer').addEventListener('click', function (e) {
    if (e.target && e.target.classList.contains('remove-dependente')) {
        e.target.closest('.dependente-item').remove();
        // Reajustar os índices dos inputs para que o model binding funcione corretamente
        reindexDependentes();
    }
});

function reindexDependentes() {
    var dependenteItems = document.querySelectorAll('#dependentesContainer .dependente-item');
    dependenteItems.forEach(function (item, index) {
        item.querySelectorAll('input').forEach(function (input) {
            var name = input.name;
            if (name) {
                input.name = name.replace(/\[\d+\]/, '[' + index + ']');
                input.id = name.replace(/\[\d+\]/, '_' + index + '__').replace(/\./g, '_');
            }
        });
        item.querySelectorAll('label').forEach(function (label) {
            var htmlFor = label.htmlFor;
            if (htmlFor) {
                label.htmlFor = htmlFor.replace(/_\d+__/, '_' + index + '__');
            }
        });
        item.querySelectorAll('span[data-valmsg-for]').forEach(function (span) {
            var dataValmsgFor = span.getAttribute('data-valmsg-for');
            if (dataValmsgFor) {
                span.setAttribute('data-valmsg-for', dataValmsgFor.replace(/\[\d+\]/, '[' + index + ']'));
            }
        });
    });
}