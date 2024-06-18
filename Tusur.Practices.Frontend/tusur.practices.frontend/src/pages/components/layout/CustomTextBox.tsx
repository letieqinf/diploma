import "./CustomTextBox.css";

interface ICustomTextBox {
    content?: string,
    name?: string,
    className?: string
}

export const CustomTextBox: React.FunctionComponent<ICustomTextBox> = ( props ) => {
    const { content, name, className } = props;

    return (
        <textarea
            name={ name }
            className={ `--custom-textbox ${ className }` }
        >
            { content }
        </textarea>
    );
}