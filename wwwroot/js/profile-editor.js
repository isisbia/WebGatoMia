// Aguarda o DOM estar completamente carregado antes de executar o script
document.addEventListener('DOMContentLoaded', function () {
    const addDependenteButton = document.getElementById('addDependente');
    const dependentesContainer = document.getElementById('dependentesContainer');

    // Função para adicionar um novo campo de dependente
    if (addDependenteButton) {
        addDependenteButton.addEventListener('click', function () {
            // Usa o número de filhos atuais como o próximo índice
            const index = dependentesContainer.children.length;

            const newDependenteHtml = `
                <div class="dependente-item mt-3 border p-3 rounded">
                    <input type="hidden" name="Dependentes[${index}].Id" value="" />
                    <div class="form-group">
                        <label for="Dependentes_${index}__Nome">Nome do Dependente</label>
                        <input type="text" id="Dependentes_${index}__Nome" name="Dependentes[${index}].Nome" class="form-control" required/>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].Nome" data-valmsg-replace="true"></span>
                    </div>
                    <div class="form-group">
                        <label for="Dependentes_${index}__DataNascimento">Data de Nascimento do Dependente</label>
                        <input type="date" id="Dependentes_${index}__DataNascimento" name="Dependentes[${index}].DataNascimento" class="form-control" required/>
                        <span class="text-danger field-validation-valid" data-valmsg-for="Dependentes[${index}].DataNascimento" data-valmsg-replace="true"></span>
                    </div>
                    <button type="button" class="btn btn-danger remove-dependente mt-2">Remover</button>
                </div>
            `;
            dependentesContainer.insertAdjacentHTML('beforeend', newDependenteHtml);

            // Reativa a validação do jQuery Unobtrusive para os novos elementos
            // Isso é importante se você estiver usando jQuery Validation e Unobtrusive Validation
            // e os novos campos precisam ser validados dinamicamente.
            // Para isso, você pode precisar incluir jQuery e jQuery Validation no seu projeto.
            // Se não estiver usando, pode remover as duas linhas abaixo.
            const newElement = dependentesContainer.lastElementChild;
            if (window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive) {
                jQuery.validator.unobtrusive.parseElement(newElement);
            }
        });
    }

    // Função para remover um campo de dependente
    if (dependentesContainer) {
        dependentesContainer.addEventListener('click', function (e) {
            if (e.target && e.target.classList.contains('remove-dependente')) {
                // Encontra o item pai do dependente e o remove
                e.target.closest('.dependente-item').remove();
                // Reajustar os índices dos inputs para que o model binding funcione corretamente
                reindexDependentes();
            }
        });
    }

    // Função para reindexar os campos dos dependentes após uma remoção
    function reindexDependentes() {
        const dependenteItems = dependentesContainer.querySelectorAll('.dependente-item');
        dependenteItems.forEach(function (item, index) {
            // Atualiza os atributos 'name' e 'id' para os inputs
            item.querySelectorAll('input').forEach(function (input) {
                const name = input.name;
                if (name) {
                    input.name = name.replace(/\[\d+\]/, '[' + index + ']');
                    input.id = name.replace(/\[\d+\]/, '_' + index + '__').replace(/\./g, '_');
                }
            });
            // Atualiza o atributo 'for' para as labels
            item.querySelectorAll('label').forEach(function (label) {
                const htmlFor = label.htmlFor;
                if (htmlFor) {
                    label.htmlFor = htmlFor.replace(/_\d+__/, '_' + index + '__');
                }
            });
            // Atualiza o atributo 'data-valmsg-for' para os spans de validação
            item.querySelectorAll('span[data-valmsg-for]').forEach(function (span) {
                const dataValmsgFor = span.getAttribute('data-valmsg-for');
                if (dataValmsgFor) {
                    span.setAttribute('data-valmsg-for', dataValmsgFor.replace(/\[\d+\]/, '[' + index + ']'));
                }
            });
        });
    }
});