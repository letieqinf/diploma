import './ChoiceMenu.css';

function ChoiceMenu(props: { titles: Array<string>, onClick }) {
    return (
        <ul className="choice-menu">
            { props.titles.map((title, index) => 
                <ChoiceMenuItem 
                    key={index}
                    title={title}
                    onClick={props.onClick}
                />
            ) }
        </ul>
    );
}

function ChoiceMenuItem(props: { title: string, onClick }) {
    return (
        <li className="choice-menu-item">
            <p onClick={props.onClick}>{ props.title }</p>
        </li>
    );
}

export default ChoiceMenu;