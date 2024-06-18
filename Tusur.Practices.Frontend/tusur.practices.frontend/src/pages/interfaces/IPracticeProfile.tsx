interface IPracticeProfile {
    contractId: string,
    studentDates: [{
        studentId: string,
        practiceDateId: string
    }],
    status: number
}

export default IPracticeProfile;