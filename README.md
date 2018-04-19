Интерфейс
Робот получает программу в виде списка команд и способ вводить строки с консоли, выполняет программу и выдает результат — то, что должно быть выведено на консоль. Программный интерфейса робота описан в проекте Robot.zip, а спецификация по его командам приведена ниже.

Спецификация команд робота
Робот умеет обрабатывать 10 следующих команд:

PUSH str — кладет на вершину стека строку str.
Строка str:

заключена в одиночные кавычки ';
на стек строка str попадает без внешних одинарных кавычек;
в частности может быть пустой и содержать пробельные символы;
символ одинарной кавычки внутри строки обозначается удвоенной одинарной кавычкой;
например, команда PUSH 'abc''def' добавит на стек строку abc'def.
POP — удаляет верхний элемент из стека.

READ — считывает строку из консоли и помещает ее на вершину стека.

WRITE — выводит на консоль строку, лежащую на вершине стека. Выведенная строка остается на вершине стека.

SWAP a b — меняет местами два элемента, лежащих на глубине a и b. Глубина верхнего элемента считается равной 1.

COPY n — копирует строку из стека на вершину. Копируется строка, лежащая на глубине n. Глубина верхнего элемента считается равной 1.

LABEL label — метка, куда можно перейти командой JMP. Строка label является непустой и не содержит пробелов. Регистр символов не имеет значения.

JMP [label] — переходит по метке label.
После выполнения команды JMP X, следующей инструкцией выполнится та, которая идёт после команды LABEL X. Если аргумент команды не указан, то переход осуществляется по метке, лежащей на вершине стека. При этом с вершины стека метка должна быть удалена.

CONCAT — конкатенирует строки. Функция принимает два аргумента, расположенные на вершине стека. Назовём их a, b. Функция убирает со стека оба аргумента и кладёт на вершину стека результат их конкатенации, т.е. строку ab.

REPLACEONE — заменяет подстроку в строке.
Функция принимает четыре аргумента, которые берет с вершины стека. Назовём их a, b, c, ret. Функция пытается заменить самое левое вхождение подстроки b на подстроку c в строке a. Если замена удалась, все 4 элемента из стека удаляются. А результат замены кладётся на вершину стека. Если замену произвести не получилось, функция из четырёх строк, взятых аргументами, оставляет только a и переходит по метке ret.

Строки, используемые в программе, не поддерживают Unicode, а их длина может достигать 1000000.
Количество команд в программе не больше 100000.

При запуске робота гарантируется, что:

команды в программе корректны,
метки, по которым существует переход, всегда существуют, и каждая метка присутствует не больше одного раза,
команда COPY всегда в качестве аргумента получает номер существующего элемента в стеке,
названия команд всегда приведены в верхнем регистре.
Однако, как и всегда в программировании, полезно озаботиться понятными сообщениями об ошибках, на случай, если что-то пойдет не так.
