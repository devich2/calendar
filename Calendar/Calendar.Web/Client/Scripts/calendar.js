import moment from 'moment'
import qs from 'qs';

moment.locale("uk");
let currentDate = null;
let currentCalendar = [];
let tasksData = null;
let deletedTasks = [];
const get = async (path, queries = {}) => {
    const getPath = () => {
        const stringifiedQueries = qs.stringify(queries, { addQueryPrefix: true, encode: false, arrayFormat: 'repeat' });
        return `${path}${stringifiedQueries}`;
    }
    const response = await fetch(getPath());
    return response.json();
}

const send = (url = "", data = {}, method = 'POST') => {
    return fetch(url, {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    }).then(res => res.json()).catch(error => {
        console.warn(error);
        setInfo("Server error: " + error,true);
    });
}

const fillTasks = data => {
    data.forEach((item, index) => {
        const dayDate = moment(item.dayDate);
        const calendarDateIndex = currentCalendar.findIndex(x => x.isSame(dayDate, "day") && x.isSame(dayDate, "month"));
        if(calendarDateIndex !== -1){
            const cellTask = document.querySelectorAll(".cell .task-count").item(calendarDateIndex);
            if(cellTask !== null){
                cellTask.textContent = item.totalCount || 0;
            }
        }
    });
}

const renderCalendar = () => {
    document.querySelectorAll(".cell .number-day").forEach((value, key) => {
        value.textContent = currentCalendar[key].format("DD");
        value.parentElement.setAttribute("index", key.toString());
        if(currentCalendar[key].isSame(moment(), "day") && currentCalendar[key].isSame(moment(), "month")){
            value.classList.add("selected");
        }
    });
}

const showCalendar = (date = moment(), func = null) => {
    const days = getAllDaysInMonth({
        year: date.year(),
        month: date.month()
    });
    document.querySelectorAll(".cell .task-count").forEach(element => {
        element.textContent = "";
    })
    currentDate = date;
    currentCalendar = days.flat();
    document.querySelector(".current-date-container").textContent = date.format("MMMM YYYY");
    renderCalendar();
    get("/task", {
        startDate: days[0][0].format("YYYY-MM-DDTHH:mm:ss"),
        endDate: days[days.length - 1][6].format("YYYY-MM-DDTHH:mm:ss")
    }).then(result => {
        if(result.responseStatusType === "Succeed"){
            tasksData = result.data;
            fillTasks(result.data);
            if(func)
                func();
        }
    });
}
const getAllDaysInMonth = ({ year, month }) => {
    const start = moment({ year, month }).weekday(0);
    const week_list = []
    for (let week = start.clone(), r = 0; r <=5 ; week.add(7, 'days'), r++) {
        const day_list = []
        for (let day = week.clone(), d = 0; d < 7; d++, day.add(1, 'day')) {
            day_list.push(day.clone())
        }
        week_list.push(day_list)
    }
    return week_list
}

const createSelectionOption = (value, text) => {
    const option = document.createElement("option");
    option.value = value;
    option.text = text;
    return option;
}
const createTextArea = (name, text) => {
    const input = document.createElement('textarea');
    input.maxLength = 5000;
    input.name = name;
    input.value = text;
    return input;
}
const createSelect = (selectedDateValue) => {
    const formatSelectionOptionValue = (value) => {
        return value.format("HH:mm");
    }
    const date = moment(selectedDateValue);
    const select = document.createElement("select");
    
    for(let start = moment(date.format("YYYY-MM-DD")), i = 0 ;  i < 48; i++, start.add(30, "minutes")){
        let dateNode = createSelectionOption(start.format("YYYY-MM-DDTHH:mm:ss"), formatSelectionOptionValue(start));
        select.append(dateNode);
        if(start.isSame(date, "minutes")) {
            dateNode.selected = true;
        }
    }
    return select;
}

const setInfo = (text, error = false) => {
    const infoBlock = document.querySelector(".info");
    if(infoBlock){
        infoBlock.style.color = error ? "red" : "green";
        infoBlock.textContent = text;
        setTimeout(() => {
            infoBlock.textContent = "";
        }, 4000);
    }
}
const getSelectedOption = (sel) => {
    let opt;
    for ( let i = 0, len = sel.options.length; i < len; i++ ) {
        opt = sel.options[i];
        if ( opt.selected === true ) {
            break;
        }
    }
    return opt;
}

const updateTask = (task) => {
    const taskNode = document.querySelector(".task-row[id='" + task.id + "']");
    if(taskNode)
    {
        const taskText = taskNode.querySelector(".task-text");
        if(taskText.value === ""){
            setInfo("Empty text", true)
        }
        else{
            const selectNode = taskNode.querySelector("select");
            const selectedOption = getSelectedOption(selectNode);
            send(`/task/${task.id}`, {
                Text: taskText.value,
                DeadLine: selectedOption.value
            }, "PUT").then(res => {
                setInfo("Successfully updated");
                task.deadLine = selectedOption.value;
                task.text = taskText.value;
            });
        }
    }
}
const deleteTask = (task) => {
    const taskDeleted = task;
    send(`/task/${taskDeleted.id}`, {}, "DELETE").then(result => {
        setInfo("Deleted");
        const taskNode = document.querySelector(".task-row[id='" + taskDeleted.id + "']");
        if(taskNode) {
            taskNode.remove();
            const taskDate = moment(taskDeleted.deadLine);
            const calendarDateIndex = currentCalendar.findIndex(x => {
                return x.isSame(taskDate, "day") && x.isSame(taskDate, "month");
            })
            if(calendarDateIndex !== -1){
                const cellTask = document.querySelectorAll(".cell .task-count").item(calendarDateIndex);
                if(cellTask !== null){
                    const newCount = parseInt(cellTask.textContent) - 1;
                    cellTask.textContent = newCount === 0 ? "" : newCount;
                }
            }
            deletedTasks.push(task.id);
        }
    })
}

const buildTaskListByDateTime = (taskData) => {
    const taskListContainer = document.querySelector(".task-list-wrapper");
    if(taskListContainer){
        if(taskData && taskData.result && taskData.result.length){
            taskData.result.forEach((task, index) => {
                if(!deletedTasks.includes(task.id)){
                    const taskNode = document.createElement("div");
                    taskNode.classList.add("task-row");
                    const select = createSelect(task.deadLine);
                    select.classList.add("task-date");
                    const textNode = createTextArea(task.id, task.text);
                    textNode.classList.add("task-text");
                    taskNode.setAttribute("id", task.id);
                    taskNode.append(select);
                    taskNode.append(textNode);
    
                    const buttonContainer = document.createElement("div");
                    buttonContainer.classList.add("action-buttons");
                    const saveNode = document.createElement("div");
                    saveNode.textContent = "Зберегти";
                    saveNode.addEventListener("click", () => {
                        updateTask(task);
                    });
                    buttonContainer.append(saveNode);

                    const deleteNode = document.createElement("div");
                    deleteNode.textContent = "Видалити";
                    deleteNode.addEventListener("click", () => {
                        deleteTask(task);
                    });
                    buttonContainer.append(deleteNode);
                    
                    taskNode.append(buttonContainer);
                    taskListContainer.append(taskNode);   
                }
            })   
        }
        else{
            const taskNode = document.createElement("div");
            taskNode.textContent = "Empty list of tasks";
            taskListContainer.append(taskNode);
        }
    }
}
const showTaskList = (e) => {
    cleanTaskList();
    const selectedCell = e.currentTarget;
    selectedCell.classList.add("selected");
    const index = parseInt(selectedCell.getAttribute("index"));
    const calendarDate = currentCalendar[index];
    currentDate = calendarDate;
    if(tasksData){
        const taskData = tasksData.find(x => {
            const dayDate = moment(x.dayDate);
            return dayDate.isSame(calendarDate, "day") && dayDate.isSame(calendarDate, "month");
        });

        buildTaskListByDateTime(taskData);   
    }
}
const getSelectedCellNode = () => {
    return document.querySelector(".cell.selected");   
}
const createTask = (date, text) => {
    if(text === ""){
        setInfo("Empty text", true);
        return;
    }
    send(`/task`, {
        Text: text,
        DeadLine: date
    }).then(x => {
        setInfo("Successfully created task");
        deletedTasks = [];
        const currentSelectedDateIndex = getSelectedCellNode().getAttribute("index");
        showCalendar(currentDate, () => {
            document.querySelectorAll(".cell").item(parseInt(currentSelectedDateIndex)).click();
        });
    })
}
const processCreate = (select, textNode) =>  {
    createTask(getSelectedOption(select).value, textNode.value);
}
const addTempTask = () => {
    const taskListContainer = document.querySelector(".task-list-wrapper");
    if(taskListContainer) {
        const taskNode = document.createElement("div");
        taskNode.classList.add("task-row");
        const select = createSelect(currentDate);
        select.classList.add("task-date");
        const textNode = createTextArea("", "");
        textNode.classList.add("task-text");
        taskNode.append(select);
        taskNode.append(textNode);

        const buttonContainer = document.createElement("div");
        buttonContainer.classList.add("action-buttons");
        const createNode = document.createElement("div");
        createNode.textContent = "Створити";
        createNode.addEventListener("click", () => {
            processCreate(select, textNode);
        });
        buttonContainer.append(createNode);

        const deleteNode = document.createElement("div");
        deleteNode.textContent = "Відмінити";
        deleteNode.addEventListener("click", () => {
            taskNode.remove();
        });
        buttonContainer.append(deleteNode);

        taskNode.append(buttonContainer);
        taskListContainer.append(taskNode);
    }
}

const hideAddButton = () => {
    document.querySelector(".add-button").style.display = "none";
}
const cleanAllSelected = () => {
    document.querySelectorAll(".cell.selected").forEach(element =>{
        element.classList.remove("selected");
    })
}
const attachClickEvents = () => {
    document.querySelector(".prev-month").addEventListener("click", previous);
    document.querySelector(".next-month").addEventListener("click", next);
    document.querySelectorAll(".cell").forEach(element => {
        element.addEventListener("click", function(e){
            document.querySelector(".add-button").style.display = "";
            cleanAllSelected();
            showTaskList(e);
        });
    });
    document.querySelector(".add-button").addEventListener("click", addTempTask);
}
const cleanTaskList = () =>{
    const taskListContainer = document.querySelector(".task-list-wrapper");
    if(taskListContainer){
        taskListContainer.innerHTML = "";
    }
}
const next = () => {
    cleanAllSelected();
    cleanTaskList();
    hideAddButton();
    showCalendar(currentDate.add(1, 'month'));
}

const previous = () => {
    cleanAllSelected();
    cleanTaskList();
    hideAddButton();
    showCalendar(currentDate.add(-1, 'month'));
}

document.addEventListener("DOMContentLoaded", function(){
    attachClickEvents();
    showCalendar();
});
